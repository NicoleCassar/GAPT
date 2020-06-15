using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FYPAllocationTest.Models;
using FYPAllocationTest.ViewModels;
using System.Text;
using System.IO;
using Microsoft.Data.SqlClient;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace FYPAllocationTest.Controllers
{ // The controller is responsible for all feature relating to the performance of allocations
    public class AllocationController : Controller
    {// Calling global variables
        private readonly IStudentRepository _studentRepository;
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly IAllocationRepository _allocationRepository;
        private readonly IPreferenceRepository _preferenceRepository;
        private string block; // Used to read data from the allocation result and perform a write onto the database
        private string connectionstring;

        // IMPORTANT
        private string serverName = "CHANGE SERVER NAME"; //NOTE: Change this variable's along with 'appsettings.json' file server name to your machine's erver name, in order to access MSSQL server.

        public AllocationController(IAllocationRepository allocationRepository, IStudentRepository studentRepository, ISupervisorRepository supervisorRepository,  IPreferenceRepository preferenceRepository, IAreaRepository areaRepository)
        { // Constructor for AllocationController
            _allocationRepository = allocationRepository;
            _studentRepository = studentRepository;
            _supervisorRepository = supervisorRepository;
            _preferenceRepository = preferenceRepository;
            _areaRepository = areaRepository;
        }
        [Authorize(Roles = "Administrator")] // Restrict access to allocations dashboard to administrators only
        public IActionResult Allocation()
        { // Due the normalization of the database schema, several tables are required to properly output allocation details
            var model = new AllocationViewModel(); // Call ViewModel for Allocations content
            model.allocation = _allocationRepository.GetAllData(); // Get all allocations
            model.student = _studentRepository.GetAllData().OrderByDescending(s => s.average_mark); // Get all student data
            model.supervisor = _supervisorRepository.GetAllData(); // Get all supervisor data
            model.preferences = _preferenceRepository.GetAllData(); // Get all student preferences 
            model.area = _areaRepository.GetAllData(); // Get all existing areas
            if (model.student.Count() == 0) // If no students exist within the students table
            {
                ViewBag.NoStudents = "No Student list has been imported yet";
                ViewBag.Ready = "false"; // Prevent 'Perform Allocations' button from being enabled
            }
            else
                ViewBag.Ready = "true"; // Enable 'Perform Allocations' button
            if (model.supervisor.Count() == 0) // If no supervisors have been imported
            {
                ViewBag.NoSupervisors = "No Supervisor list has been imported yet"; 
                ViewBag.Ready = "false"; // Prevent 'Perform Allocations' button from being enabled
            }
            else
                ViewBag.Ready = "true"; // Enable 'Perform Allocations' button
            if (model.preferences.Count()/ 6 != model.student.Count()) // If number of students who have submitted preferences is not equal to the total number of students
            {
                ViewBag.NotSubmitted = "Some Students haven't submitted their preferences yet";
                ViewBag.Ready = "false"; // Prevent 'Perform Allocations' button from being enabled
            }
            else
                ViewBag.Ready = "true"; // Enable 'Perform Allocations' button
            if (model.allocation.Count() == 0) // If no allocations exist with the allocations table
            {
                ViewBag.Unavailable = "No Allocations have been performed yet"; // inform administrator that no allocations have been performed
            }
            else // If all criteria are met and allocations have already been performed
            {
                foreach (var students in model.student) // Loop through all students
                {
                    bool exists = false;
                    foreach (var allocations in model.allocation) // Loop through existing allocations
                    {
                        if (allocations.student_id == students.student_id) // Check to see if a given student exists on the allocations table 
                        {
                            exists = true;
                        }
                    }
                    if (!exists) // If a student is found to have not yet been allocated
                    {
                        ViewBag.NotFound = "Some Students remain unallocated, please allocate them through the below link";
                        ViewBag.Unassigned = "true"; // Prevent the 'Publish Results' button from being enabled
                    }
                    else // If all students have been successfully allocated
                    {
                        ViewBag.Assigned = "All Students have been allocated, the option to publish results is now available";
                        ViewBag.Complete = "true"; // Enable 'Publish Results' button
                    }
                }
                ViewBag.Ready = "false"; // Disable 'Perform Allocations' button after allocation has been performed
                ViewBag.Performed = "true"; // Make Allocations log available for download 

            }
            ViewBag.Message = TempData["Not Assigned"]; // Set notification in the case of students not being assigned after allocation
            ViewBag.Success = TempData["Success"]; // Notify successful allocation
            return View(model);
            
        }

        [Authorize(Roles = "Administrator")] // Restrict dashboard access to Administrators
        public IActionResult Dashboard()
        {
            ViewBag.success = TempData["success"]; // Set up notification to inform administrators of successful operations
            return View();
        }

        [Authorize(Roles = "Administrator")] // Restrict downloading of Supervisor and Area information to Administrators
        public FileResult Export_Supervisors() // This method is responsible for the exportation of Supervisor and Area details from the database
        {
            List<string> columnData = new List<String>(); // Create a list for column data
            connectionstring = "Server=" + serverName +";Database=fypallocation;Trusted_Connection=True;MultipleActiveResultSets=true"; // Set up a connection string with the database details
            using (SqlConnection connection = new SqlConnection(connectionstring)) // Initiate connection
            {
                connection.Open(); // Open Connection and execute the following INNER JOIN query
                string query = "SELECT sup.name, sup.surname, sup.supervisor_id, area.title, sup.quota, area.available, area.area_quota, area.area_id FROM supervisor sup INNER JOIN supervisor_area area ON sup.supervisor_id = area.supervisor_id ORDER BY sup.quota;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader()) // For each column in the database, save data as a block to an element of columnData
                    {
                        while (reader.Read())
                        {
                            columnData.Add(reader[0].ToString()
                                + " " + reader[1].ToString()
                                + "," + reader[2].ToString()
                                + "," + reader[3].ToString()
                                + "," + reader[4].ToString()
                                + "," + reader[5].ToString()
                                + "," + reader[6].ToString()
                                + "," + reader[7].ToString());
                        }
                    }
                }
            }
            var sw = new StreamWriter("wwwroot\\csv\\supervisors.csv", false, Encoding.ASCII); // Open write stream to supervisors csv file
            foreach (var item in columnData) // Write all the saved blocks to the csv file
                sw.WriteLine(item);
            sw.Close(); // Always important to close the stream
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"wwwroot\csv\supervisors.csv"); // Create byte array with contents of supervisor csv file
            string fileName = "supervisors.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName); // Download the file
        }

        [Authorize(Roles = "Administrator")]
        public FileResult Export_Students() // This method exports student data to be downloaded from the database
        {
            List<string> columnData = new List<String>(); // Set up list to store data
            connectionstring = "Server=" + serverName + ";Database=fypallocation;Trusted_Connection=True;MultipleActiveResultSets=true"; // Set up the connection string
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open(); // Open database connection and execute INNER JOIN QUERY
                string query =
                    "SELECT pref.preference_id, stud.name, stud.surname, stud.average_mark, stud.student_id, sup.name, sup.surname, area.title " +
                    "FROM student_preference pref " +
                    "INNER JOIN supervisor_area area " +
                    "ON pref.area_id = area.area_id " +
                    "INNER JOIN supervisor sup " +
                    "ON area.supervisor_id = sup.supervisor_id " +
                    "INNER JOIN student stud " +
                    "ON pref.student_id = stud.student_id " +
                    "ORDER BY stud.average_mark DESC, stud.student_id, pref.preference_id, pref.time_submitted ;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader()) // Initiate reader to save database contents
                    {
                        while (reader.Read()) // While not all data has been read
                        {
                            columnData.Add(reader[0].ToString() // Add the rows for each column to an element in the columnData list
                                + "," + reader[1].ToString()
                                + " " + reader[2].ToString()
                                + "," + reader[3].ToString()
                                + "," + reader[4].ToString()
                                + "," + reader[5].ToString()
                                + " " + reader[6].ToString()
                                + "," + reader[7].ToString());
                        }
                    }
                }
            }
            var sw = new StreamWriter("wwwroot\\csv\\students.csv", false, Encoding.ASCII); 
            foreach (var item in columnData)
                sw.WriteLine(item); // Write list contents to students.csv file
            sw.Close(); // Always remember to close the stream
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"wwwroot\csv\students.csv"); // Store csv file as bytes array
            string fileName = "students.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName); // Download the csv file
        }

        [Authorize(Roles = "Administrator")]
        public FileResult Export_Log() // Allows administrators to download a text file containing a log generated by the allocation algorithm
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"wwwroot\\txt\\Allocation_Log.txt"); // Save log to 'Allocation_Log' text file
            string fileName = "Allocation_Log.txt";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName); // Commence download
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Perform_Allocation() // Actual method for executing allocation algorithm and storing results
        {
            Stopwatch sw = new Stopwatch(); // Setting Benchmark for analysis
            sw.Start(); //Start timer
            _allocationRepository.Delete(); // Purge current allocations before performing new allocation, this is in place even though allocation may not be performed twice.
            AreaCode(); // Generate Area Code for each supervisor, to be used in the output of allocation results
            Export_Supervisors(); // Get supervisor and area data from the database
            Export_Students(); // Get student data from the database
            ScriptEngine engine = Python.CreateEngine(); // Call IrnPython extension in order to make calls to python files
            engine.ExecuteFile(@"Python\Allocation.py"); // Execute the allocations algorithm stored within the Python folder
            if (System.IO.File.Exists("wwwroot\\csv\\allocation_result.csv")) // If the allocation succeeded and results were subsequently stored into the appropriate csv file
            {
                StreamReader sr = new StreamReader("wwwroot\\csv\\allocation_result.csv");
                // Initialise a series of lists to hold each item of data to be passed to the database
                List<string> stud_id = new List<string>(); 
                List<string> sup_id = new List<string>();
                List<string> res = new List<string>();
                
                while ((block = sr.ReadLine()) != null) // For each block of data in the file
                {
                    res.Add(block); // Add grouped contents to a list
                }
                var result = res.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList(); // For validation purposes, trim any possible whitespace that may have been generated
                for (int i = 0; i <= result.Count()-1; i++) // For every block now stored in the list, being each recorded allocation.
                {
                    var cell = result[i].Split(','); // remove the commas added by definition of a csv and add each value found between the commas as an individual element
                    stud_id.Add(cell[1]); // Extract student ids from the csv result file
                    
                    if (cell[5] == "None") // The allocation algorithm will set unassigned students with the value 'None' to inform the controller that a given student has not been assigned
                    { // Set up a notification to inform administrator of lack of allocation
                       TempData["Not Assigned"] = "Notice: Some student/s were not assigned due to lack of availability. remaining student may be allocated through the link below:";
                    }
                    else
                    { // If student has been allocated successfully
                        sup_id.Add(cell[5]); // Add value from csv cell to list of supervisor ids
                        Supervisor_Update(cell[5], Convert.ToInt32(cell[6])); // Update supervisor table with new quota for each supervisor
                        Area_Update(Convert.ToInt32(cell[3]), Convert.ToInt32(cell[7])); // Update areas table with new area quota for each area
                        SaveAlloc(stud_id[i], sup_id[i]); // Save allocations to database
                    }   
                }
                sr.Close(); // Always close the stream
                sw.Stop(); //Stop benchmarking
                Console.WriteLine("\nTime was: " + sw.ElapsedMilliseconds/1000 + " seconds"); //Output Benchmarked time onto console, for testing purposes
                return RedirectToAction("Allocation"); // Refresh allocations page
            }
            else
            {
                TempData["Not Assigned"] = "An error occured during allocation"; // If something goes wrong during the allocation process
                return RedirectToAction("Allocation");
            }
        }
 
        public void SaveAlloc(string stud_id, string supervisor_id) // This method will store each allocation within the database
        {
            Allocation alloc = new Allocation() // Set up new object of type Allocation
            {
                student_id = stud_id, 
                supervisor_id = supervisor_id,
                manual = false
            };
            _allocationRepository.Create(alloc); // Call to repository to save allocation
        }

        public void Supervisor_Update(string id, int sup_quota) // Updating of supervisor quota
        {
            var current_supervisor = _supervisorRepository.GetSupervisorById(id); // Find the supervisor to be updated
            Supervisor new_quota = new Supervisor() // Set up a new object of type supervisor
            {
                supervisor_id = current_supervisor.supervisor_id,
                quota = sup_quota
            };
            _supervisorRepository.UpdateQuota(new_quota); // Call to repository in order to update quota
        }

        public void Area_Update(int id, int ar_quota) // Update area quotas for each area used during allocation
        {
            var current_area = _areaRepository.GetAreaById(id); // Get the area required for update
            Area new_quota = new Area() // Set up a new object of type Area
            {
                area_id = current_area.area_id,
                area_quota = ar_quota
            };
            _areaRepository.UpdateQuota(new_quota); // Call repository to update area quota
        }

        public void AreaCode()
        {//This method will generate Serial codes per area and supervisor combination, as a means of compressing more inforamtion iwth regards to result onto the allocation table
            var areas = _areaRepository.GetAllData();
            var supervisors = _supervisorRepository.GetAllData();
            List<Area> code = new List<Area>();
            foreach(var supervisor in supervisors) // Iterate through supervisors
            {
                int i = 1; // iterator set to add numbering to each available area of any given supervisor
                foreach (var area in areas) // Go through all areas for a supervisor
                {
                    if (area.supervisor_id == supervisor.supervisor_id) // If area's Foreign Key for supervisor id matches the id of the supervisor  
                    { // Generate a code for the particular area that is made up of the area number, supervisor initials and supervisor quota
                        code.Add(new Area { area_id = area.area_id ,area_code = i.ToString() + supervisor.name[0] + supervisor.surname[0] + supervisor.quota});
                        i++; // Increment i to asign the next area belonging to the supervisor with an appropriate number
                    }       
                }
            }
            foreach(var item in code) // Get all the codes generated and store them within the database
            {
                _areaRepository.AddAreaCodes(item); // Call the repository to add codes to each area
            }
        }
    }
}


