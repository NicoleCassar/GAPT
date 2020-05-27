using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FYPAllocationTest.Models;
using FYPAllocationTest.ViewModels;
using System.Text;
using System.IO;
using Microsoft.Data.SqlClient;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace FYPAllocationTest.Controllers
{
    public class AllocationController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly IAllocationRepository _allocationRepository;
        private readonly IPreferenceRepository _preferenceRepository;

        public AllocationController(IAllocationRepository allocationRepository, IStudentRepository studentRepository, ISupervisorRepository supervisorRepository,  IPreferenceRepository preferenceRepository, IAreaRepository areaRepository)
        {
            _allocationRepository = allocationRepository;
            _studentRepository = studentRepository;
            _supervisorRepository = supervisorRepository;
            _preferenceRepository = preferenceRepository;
            _areaRepository = areaRepository;
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult Allocation()
        {
            var model = new AllocationViewModel();
            model.allocation = _allocationRepository.GetAllData();
            model.student = _studentRepository.GetAllData().OrderByDescending(s => s.average_mark);
            model.supervisor = _supervisorRepository.GetAllData();
            model.preferences = _preferenceRepository.GetAllData();
            model.area = _areaRepository.GetAllData();
            if (model.student.Count() == 0)
            {
                ViewBag.NoStudents = "No Student list has been imported yet";
                ViewBag.Ready = "false";
            }
            else
                ViewBag.Ready = "true";
            if (model.supervisor.Count() == 0)
            {
                ViewBag.NoSupervisors = "No Supervisor list has been imported yet";
                ViewBag.Ready = "false";
            }
            else
                ViewBag.Ready = "true";
            if (model.preferences.Count()/ 6 != model.student.Count())
            {
                ViewBag.NotSubmitted = "Some Students haven't submitted their preferences yet";
                ViewBag.Ready = "false";
            }
            else
                ViewBag.Ready = "true";
            if (model.allocation.Count() == 0)
            {
                ViewBag.Unavailable = "No Allocations have been performed yet";
            }
            else
            {
                foreach (var students in model.student)
                {
                    bool exists = false;
                    foreach (var allocations in model.allocation)
                    {
                        if (allocations.student_id == students.student_id)
                        {
                            exists = true;
                        }
                    }
                    if (!exists)
                    {
                        ViewBag.NotFound = "Some Students remain unallocated, please allocate them through the below link";
                        ViewBag.Unassigned = "true";
                    }
                }
                ViewBag.Ready = "false";
                ViewBag.Performed = "true";

            }
            ViewBag.Message = TempData["Not Assigned"];
            ViewBag.Success = TempData["Success"];
            return View(model);
            
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Dashboard()
        {
            ViewBag.success = TempData["success"];
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public FileResult Export_Supervisors()
        {
            List<String> columnData = new List<String>();
            string connectionstring = "Server=MSI-CSF;Database=fypallocation;Trusted_Connection=True;MultipleActiveResultSets=true";
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                string query = "SELECT sup.name, sup.surname, sup.supervisor_id, area.title, sup.quota, area.available, area.area_quota, area.area_id FROM supervisor sup INNER JOIN supervisor_area area ON sup.supervisor_id = area.supervisor_id ORDER BY sup.quota;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
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
            var sw = new StreamWriter("supervisors.csv", false, Encoding.ASCII);
            foreach (var item in columnData)
                sw.WriteLine(item);
            sw.Close();
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"supervisors.csv");
            string fileName = "supervisors.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [Authorize(Roles = "Administrator")]
        public FileResult Export_Students()
        {
            List<String> columnData = new List<String>();
            string connectionstring = "Server=MSI-CSF;Database=fypallocation;Trusted_Connection=True;MultipleActiveResultSets=true";
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
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
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columnData.Add(reader[0].ToString()
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
            var sw = new StreamWriter("students.csv", false, Encoding.ASCII);
            foreach (var item in columnData)
                sw.WriteLine(item);
            sw.Close();
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"students.csv");
            string fileName = "students.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [Authorize(Roles = "Administrator")]
        public FileResult Export_Log()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"Allocation_Log.txt");
            string fileName = "Allocation_Log.txt";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Perform_Allocation()
        {
            Stopwatch sw = new Stopwatch(); //Setting Benchmark for analysis
            sw.Start(); //Start timer
            _allocationRepository.Delete(); //Purge current allocations before performing new allocation
            Export_Supervisors();
            Export_Students();
            ScriptEngine engine = Python.CreateEngine();
            engine.ExecuteFile(@"Allocation.py");
            if (System.IO.File.Exists("allocation_result.csv"))
            {
                StreamReader sr = new StreamReader("allocation_result.csv");              
                List<string> stud_id = new List<string>();
                List<string> sup_id = new List<string>();
                List<string> res = new List<string>();
                List<string> output = new List<string>();
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    res.Add(line);
                }
                var result = res.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                for (int i = 0; i <= result.Count()-1; i++)
                {
                    var cell = result[i].Split(',');
                    stud_id.Add(cell[1]);
                    
                    if (cell[5] == "None")
                    {
                       TempData["Not Assigned"] = "Notice: Some student/s were not assigned due to lack of availability. remaining student may be allocated through the link below:";
                    }
                    else
                    {
                        sup_id.Add(cell[5]);
                        Supervisor_Update(cell[5], Convert.ToInt32(cell[6]));
                        Area_Update(Convert.ToInt32(cell[3]), Convert.ToInt32(cell[7]));
                        SaveAlloc(stud_id[i], sup_id[i]);
                    }   
                }
                sr.Close();
                sw.Stop(); //Stop benchmarking
                Console.WriteLine("\nTime was: " + sw.ElapsedMilliseconds/1000 + " seconds"); //Output Benchmarked time
                return RedirectToAction("Allocation");
            }
            else
            {
                TempData["Not Assigned"] = "An error occured during allocation";
                return RedirectToAction("Allocation");
            }
        }

 
        public void SaveAlloc(string stud_id, string supervisor_id)
        {
            Allocation alloc = new Allocation()
            {
                student_id = stud_id,
                supervisor_id = supervisor_id,
                manual = false
            };
            _allocationRepository.Create(alloc);
        }

        public void Supervisor_Update(string id, int sup_quota)
        {
            var current_supervisor = _supervisorRepository.GetSupervisorById(id);
            Supervisor new_quota = new Supervisor()
            {
                supervisor_id = current_supervisor.supervisor_id,
                quota = sup_quota
            };
            _supervisorRepository.UpdateQuota(new_quota);
        }

        public void Area_Update(int id, int ar_quota)
        {
            var current_area = _areaRepository.GetAreaById(id);
            Area new_quota = new Area()
            {
                area_id = current_area.area_id,
                area_quota = ar_quota
            };
            _areaRepository.UpdateQuota(new_quota);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

/* 1. Get Supervisor by ID
 * 2. Get Area By ID
 * 3. Save new quotas to objects Supervisor and Area
 * 4. Send Objects to Update Methods.
 * 5. Get Area and Supervisor by ID
 * 6. Set the existing to the new quota
 * 7. _appDbContext.Entry(existingBlogPost).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
 * 8. SaveChanges()
 */
