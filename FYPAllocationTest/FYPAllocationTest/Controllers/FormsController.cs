using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYPAllocationTest.ViewModels;
using FYPAllocationTest.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace FYPAllocationTest.Controllers
{
    public class FormsController : Controller
    {
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly IPreferenceRepository _preferenceRepository;
        private readonly IStudentRepository _studentRepository;
        private List<Supervisor> sup;
        private List<Area> ar;


        public FormsController(ISupervisorRepository supervisorRepository, IAreaRepository areaRepository, IPreferenceRepository preferenceRepository, IStudentRepository studentRepository)
        {
            _supervisorRepository = supervisorRepository;
            _areaRepository = areaRepository;
            _preferenceRepository = preferenceRepository;
            _studentRepository = studentRepository;
            var supervisors = _supervisorRepository.GetAllData().OrderByDescending(p => p.supervisor_id);
            var areas = _areaRepository.GetAllData().OrderByDescending(p => p.area_id);
            sup = new List<Supervisor>();
            ar = new List<Area>();
            foreach (var item in supervisors)
            {
                sup.Add
                (
                    new Supervisor()
                    {
                        supervisor_id = item.supervisor_id, name = item.name + " " + item.surname
                    }
                );
            }
            foreach (var item in areas)
            {
                ar.Add
                (
                    new Area()
                    {
                        supervisor_id = item.supervisor_id, title = item.title
                    }
                );
            }
        }

        public IActionResult FormA()
        {
            var model = new AddStudent();
            ViewBag.supervisors = new SelectList(sup, "supervisor_id", "name");
            return View(model);
        }

        public IActionResult Import()
        {
            var model = new Imports();
            return View(model);
        }

        [HttpPost]
        public IActionResult FormA([Bind("name", "id", "sup1", "pref1", "sup2", "pref2", "sup3", "pref3", "sup4", "pref4", "sup5", "pref5", "sup6", "pref6")] AddStudent submission)
        {
            
            if (ModelState.IsValid)
            {
                
                Console.WriteLine(submission.pref1);
                var area1 = _preferenceRepository.GetAreaById(submission.pref1);
                var area2 = _preferenceRepository.GetAreaById(submission.pref2);
                var area3 = _preferenceRepository.GetAreaById(submission.pref3);
                var area4 = _preferenceRepository.GetAreaById(submission.pref4);
                var area5 = _preferenceRepository.GetAreaById(submission.pref5);
                var area6 = _preferenceRepository.GetAreaById(submission.pref6);
                List<int> areas = new List<int>()
                { area1.area_id, area2.area_id, area3.area_id, area4.area_id, area5.area_id, area6.area_id};
                List<string> supervisors = new List<string>()
                {submission.sup1, submission.sup2, submission.sup3, submission.sup4, submission.sup5, submission.sup6 };
                bool isUnique = supervisors.Distinct().Count() == supervisors.Count();
                if (isUnique)
                {
                    int i = 1;
                    foreach (var item in areas)
                    {
                        Preference preferences = new Preference()
                        {
                            preference_id = i,
                            student_id = submission.id,
                            area_id = item,
                            time_submitted = DateTime.UtcNow


                        };

                        _preferenceRepository.Submit(preferences);
                        i++;
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("All", "Cannot choose the same supervisor more than once, please re-enter");
                    ViewBag.supervisors = ViewBag.supervisors = new SelectList(sup, "supervisor_id", "name");
                    return View();
                }

               
                
            }
            else
            {
                ModelState.AddModelError("All", "Something went wrong, please try again");
                ViewBag.supervisors = ViewBag.supervisors = new SelectList(sup, "supervisor_id", "name");
                return View();
            }

        }

        public IActionResult StaffForm()
        {
            return View();
        }

        public JsonResult GetArea(string supervisor_id)
        {
            return Json(ar.Where(a => a.supervisor_id == supervisor_id));
        }

        [HttpPost]
        public IActionResult Import([Bind("studentimport")] Imports imports)
        {
            string filename = "";
            bool uploaded;
            if(ModelState.IsValid)
            {
                if (imports.studentimport != null)
                {
                    var extension = "." + imports.studentimport.FileName.Split('.')[imports.studentimport.FileName.Split('.').Length - 1];
                    Console.WriteLine(extension);
                    if (extension.ToLower() == ".csv")
                    {
                        filename = imports.studentimport.FileName;
                        var path = Directory.GetCurrentDirectory() + "\\wwwroot\\uploads\\" + filename;
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            imports.studentimport.CopyTo(stream);
                        }
                        uploaded = Import_Students(path);
                    }
                    else
                    {
                        ModelState.AddModelError("studentimport", "Invalid File type uploaded (please upload .csv file)");
                        return View(imports);
                    }
                }
                else
                {
                    ModelState.AddModelError("studentimport", "No csv file selected");
                    return View();
                }
                if(uploaded)
                {
                    TempData["success"] = "csv file successfully uploaded";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.failure = "An error occurred when trying to insert csv data, data may already exist.";
                    return View();
                }
                
            }
            else
            {
                ModelState.AddModelError("studentimport", "Invalid File uploaded (please upload .csv file)");
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public bool Import_Students(String path)
        {
            if (System.IO.File.Exists(path)) //Checking that the file exists at the given path
            {
                StreamReader sr = new StreamReader(path);
                List<string> res = new List<string>();
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    res.Add(line);
                }
                var result = res.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                for (int i = 0; i <= result.Count() - 1; i++)
                {
                    var cell = result[i].Split(',');
                    Student students = new Student() //Grabbing students from csv file by taking data from each cell
                    {
                        student_id = cell[0],
                        name = cell[1],
                        surname = cell[2],
                        email = cell[3],
                        average_mark = Convert.ToDouble(cell[4])
                    };
                    bool uploaded = _studentRepository.Import(students); //Sending retrived details to the database
                    if (!uploaded)
                    {
                        sr.Close();
                        System.IO.File.Delete(path);
                        return uploaded;
                    }
                    
                }
                sr.Close();
                System.IO.File.Delete(path);
                return true;
            }
            else
            {
                ViewBag.failure = "There was a problem adding csv data, please check document formatting is in the order id, name, surname, email, average mark";
                return false;
            }
            
        }


    }
}