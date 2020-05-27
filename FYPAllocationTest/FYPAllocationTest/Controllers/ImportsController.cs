using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYPAllocationTest.ViewModels;
using FYPAllocationTest.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace FYPAllocationTest.Controllers
{
    public class ImportsController : Controller //This controller is responsible for all actions relating to uploading of data.
    {
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IStudentRepository _studentRepository;



        public ImportsController(ISupervisorRepository supervisorRepository, IStudentRepository studentRepository)
        {
            _supervisorRepository = supervisorRepository;
            _studentRepository = studentRepository;
           
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Import() //Loading in view for importing students and supervisors
        {
            ViewBag.failurestudent = TempData["failurestudent"]; //Each TempData is set as a custom validation for students' and supervisors' success or failure.
            ViewBag.failuresupervisor = TempData["failuresupervisor"];
            ViewBag.studentimport = TempData["studentimport"];
            ViewBag.supervisorimport = TempData["supervisorimport"];
            var model = new Imports(); //Using the Imports View Model
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Import_Student([Bind("studentimport")] Imports imports) //Importing CSV for students
        {
            string path; //File path extracted from uploaded file
            bool uploaded; //Boolean validator to handle any caught upload or saving exceptions
            var tobeuploaded = imports.studentimport; //Saving the file uploaded to be sent as a parameter
            if (ModelState.IsValid)
            { //Parameters for Upload are 1 or 0 for enumeration, and the file uploaded.
                path = Upload(tobeuploaded); //Calling the Upload() method in order to upload the file submitted
                if (path == "Invalid") //If the Upload() method returns "Invalid", this means that the submitted file is NOT of type .csv
                {
                    TempData["studentimport"] = "Invalid File type uploaded (please upload .csv file)."; //Prepare custom validation to inform user of incorrect upload
                    return RedirectToAction("Import"); //Redirect to Imports view with validation message
                }
                else if (path == "None") //If the Upload() method returns "None" this means that no file was uploaded.
                {
                    TempData["studentimport"] = "No csv file selected."; //Prepare custom validation to inform the user that nothing was submitted.
                    return RedirectToAction("Import");//Redirect to Imports view with validation message
                }
                else //If the actual file path was returned, then upload was successful.
                {
                    uploaded = Import_DB(0, path); //Call the method to write to the database with parameters for enumeration to sort students from supervisors, and the file path to be extrapolated.
                } //The Import_DB() method returns a boolean value to handle the case of catching an exception on SQL insertion failure.
                if (uploaded) //If upload is successful
                {
                    TempData["success"] = "student data successfully uploaded from csv file"; //Prepare a success message to ensure the user of task completion
                    return RedirectToAction("Dashboard", "Allocation"); //Return to the desired page with the success message
                }
                else //if the data contains already existing elements, or the column sequence from the csv does not match the database column sequence.
                {
                    TempData["failurestudent"] = "An error occurred, csv columns be in the wrong order or already exist. Only new data was inserted";  //Prepare a custom error message to inform the user of failure
                    return RedirectToAction("Import"); //return to the import page, giving user advice on what caused the failure
                }

            }
            else //If the mofel state proves to be invaid
            {
                TempData["studentimport"] = "Invalid File type uploaded (please upload .csv file)."; //Prepare a validation message
                return RedirectToAction("Import"); //return to Import view.
            }
        }


        [Authorize(Roles = "Administrator")]
        [HttpPost] //Similar to students, supervisor is tasked to perform imports of supervisor, with only the type of data being processed differenciating the two methods.
        public IActionResult Import_Supervisor([Bind("supervisorimport")] Imports imports)
        {
            string path; //File path extracted from uploaded file
            bool uploaded; //Boolean validator to handle any caught upload or saving exceptions
            var tobeuploaded = imports.supervisorimport; //Saving the file uploaded to be sent as a parameter
            if (ModelState.IsValid)//Parameters for Upload are 1 or 0 for enumeration, and the file uploaded.
            {
                path = Upload(tobeuploaded); //Supervisor data will be passed to the Upload() Method in order for the file to be uploaded to the server
                if(path == "Invalid") //If the Upload() method returns "Invalid", this means that the submitted file is NOT of type .csv
                {
                    TempData["supervisorimport"] = "Invalid File type uploaded (please upload .csv file).";  //Prepare custom validation to inform user of incorrect upload
                    return RedirectToAction("Import"); //Redirect to Imports view with validation message
                }
                else if(path == "None") //If the Upload() method returns "None" this means that no file was uploaded.
                {
                    TempData["supervisorimport"] = "No csv file selected."; //Prepare custom validation to inform the user that nothing was submitted.
                    return RedirectToAction("Import"); //Redirect to Imports view with validation message
                }
                else //If the actual file path was returned, then upload was successful.
                {
                    uploaded = Import_DB(1, path); //Calling the databse insertion method, with 1 to identify supervisor data, and the appropriate csv file path.
                }
                if (uploaded) //If supervisor upload was a success
                {
                    TempData["success"] = "supervisor data successfully uploaded from csv file"; //Prepare a message to inform the user of success
                    return RedirectToAction("Dashboard", "Allocation"); //Redirect to the desired page and display success message
                }
                else //if any error occired whilst inserting supervisor data, due to the same possible exceptions as the student import.
                {
                    TempData["failuresupervisor"] = "An error occurred, csv columns be in the wrong order or already exist. Only new data was inserted"; //Prepare a failure message
                    return RedirectToAction("Import"); //Return to Import view with a failure message.
                }

            }
            else //The the model state for supervisors proves to be invalid 
            {
                TempData["supervisorimport"] = "Invalid File type uploaded (please upload .csv file)."; //Inform the user of an invalid state
                return RedirectToAction("Import"); //Return with message.
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public bool Import_DB(int id, String path) //This method deals with inserting csv data into the database
        {
            if (System.IO.File.Exists(path)) //Checking that the file exists at the given path
            {
                StreamReader sr = new StreamReader(path); //Open read stream for csv file
                List<string> res = new List<string>(); //similiar to processing allocated students, csv data will be read cell by cell
                string row;
                while ((row = sr.ReadLine()) != null) // Read and display rows from the file until the end of the file is reached.
                {
                    res.Add(row); //Add each row to the list res
                }
                var result = res.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList(); //eleminate any possible whitespace that may have been created during processing
                for (int i = 0; i <= result.Count() - 1; i++) //For each row in the csv file, now inside the res list
                {
                    var cell = result[i].Split(','); //split each comma delimited string into multiple list elements
                    if (id == 0) //if enumaeration indicates student data is to be inserted
                    {
                        Student students = new Student() //Grabbing students from csv file by taking data from each cell
                        {
                            student_id = cell[0],
                            name = cell[1],
                            surname = cell[2],
                            email = cell[3],
                            average_mark = Convert.ToDouble(cell[4])
                        }; 
                        bool uploaded = _studentRepository.Import(students); //Sending retrived details to the database
                        if (!uploaded) //if upload fails
                        {
                            sr.Close(); //close the stream
                            System.IO.File.Delete(path); //delete the csv file from storage (for space optimisation purposes).
                            return uploaded; //return false to the import method for students.
                        }
                    }
                    if (id == 1) //If enumeration indicates that supervisor data is to be inserted
                    {
                        Supervisor supervisors = new Supervisor() //Grabbing supervisors from csv file by taking data from each cell
                        {
                            supervisor_id = cell[0],
                            name = cell[1],
                            surname = cell[2],
                            email = cell[3],
                            quota = Convert.ToInt32(cell[4])
                        };
                        bool uploaded = _supervisorRepository.Import(supervisors); //Sending retrived details to the database
                        if (!uploaded) //if upload is not succesful 
                        {
                            sr.Close(); //Always close the stream, as not doing so will cause an exception if user attempts to reupload
                            System.IO.File.Delete(path); //Remove the uploaded csv from the server (for space optimisation purposes).
                            return uploaded; //return false to indicate upload failure
                        }
                    }
                }
                sr.Close(); //Even if success, ALWAYS close the stream.
                System.IO.File.Delete(path); //Once again, delete the uploaded file now that data has been successfully utilised.
                return true; //Return true to indicate successful upload.
            }
            else //if the file specified does not exist
            {
                return false; //Return false to indicate failed upload
            }
        }

        public String Upload(IFormFile imports) //Method that handles the upload of files to the server
        { //mw
            string filename; //filename will contain the name part of the imported file.
            string path; //will specify the full file path
            if (imports != null) //if the file field is populated upon submission
            {
                var extension = "." + imports.FileName.Split('.')[imports.FileName.Split('.').Length - 1]; //retrieve the .csv extension of the file for validation
                if (extension.ToLower() == ".csv") //ensure that a .csv file has been uploaded
                {
                    filename = imports.FileName; //store the file name inside filename
                    path = Directory.GetCurrentDirectory() + "\\wwwroot\\uploads\\" + filename; //put together the full path in which the file will be stored on the server until dthe database is successfully populated
                    using (var stream = new FileStream(path, FileMode.Create)) //open a stream in order to insert the file given into the desired path
                    {
                        imports.CopyTo(stream); //using the Copyto() call in order to write to the stream
                    }
                }
                else //if the file uploaded is not of type .csv
                {
                    return "Invalid"; //return code invalid to be processed accrodingly
                }
                return path; //if successful, provide the location of the uploaded file to be used for insertion by means of the path string
            }
            else
                return "None"; //if the form was submitted with no file attached.
        }
    }
}