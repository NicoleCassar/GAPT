using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FYPAllocationTest.ViewModels;
using FYPAllocationTest.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FYPAllocationTest.Controllers
{ // This controller is responsible for the compilation of forms to be submitted
    public class FormsController : Controller
    { // Setting global variables for controller
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly IPreferenceRepository _preferenceRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private List<Supervisor> sup;
        private List<Area> ar;
        
        public FormsController(ISupervisorRepository supervisorRepository, IAreaRepository areaRepository, IPreferenceRepository preferenceRepository, IStudentRepository studentRepository, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        { // This contructor will server to compile data for the means of implementing cascading dropdown lists
            _supervisorRepository = supervisorRepository;
            _areaRepository = areaRepository;
            _preferenceRepository = preferenceRepository;
            _studentRepository = studentRepository;
            _signinManager = signInManager;
            _userManager = userManager;
            var supervisors = _supervisorRepository.GetAllData().OrderByDescending(p => p.supervisor_id); // Get all supervisor data
            var areas = _areaRepository.GetAllData().OrderByDescending(p => p.area_id); // Get all area data
            sup = new List<Supervisor>();
            ar = new List<Area>();
            foreach (var supervisor in supervisors) // For each existing supervisor
            {
                int count = 0; // Set count to check if a supervisor has failed to submit any areas
                foreach (var area in areas) // Go through areas and count those associated with the supervisor
                {
                    if(supervisor.supervisor_id == area.supervisor_id)
                    {
                        count++;
                    }
                }
                if(count != 0) // If areas associated with the supervisor exist
                {
                    sup.Add // Add supervisor to list of supervisors
                    (
                        new Supervisor()
                        {
                            supervisor_id = supervisor.supervisor_id,
                            name = supervisor.name + " " + supervisor.surname
                        }
                    );
                }
                
            }
            foreach (var area in areas) // Add areas associated with supervisor to areas list
            {
                ar.Add
                (
                    new Area()
                    {
                        supervisor_id = area.supervisor_id, 
                        title = area.title
                    }
                );
            }
        }

        [Authorize(Roles = "Student")] // Only allow students to access preference submission form
        public IActionResult FormA()
        {
            try
            {
                var model = new AddStudent(); // Using the 'AddStudent' ViewModel
                ViewBag.supervisors = new SelectList(sup, "supervisor_id", "name"); // Populate supervisor dropdown with the supervisos found in list 'sup'
                var students = _studentRepository.GetAllData(); // Get all student data
                var user = User.Identity.Name; // Get current user name
                foreach (var student in students) // for each student 
                {
                    if (user == student.email) // if the user is found on the student table
                    {
                        model.name = student.name + " " + student.surname; // Automatically fill the name field with student name
                        model.id = student.student_id; // Automatically fill the id field with the sutdent id
                    }
                }
                return View(model);
            }
            catch
            {
                return RedirectToAction("Error", "Home"); // If any error occurs, redirect to the home page
            }
            
        }

        [HttpPost]
        [Authorize(Roles = "Student")] // Only allow students to submit a form, in the case any possible malacious attempt is made to access this feature
        public IActionResult FormA([Bind("name", "id", "sup1", "pref1", "sup2", "pref2", "sup3", "pref3", "sup4", "pref4", "sup5", "pref5", "sup6", "pref6")] AddStudent submission)
        {  // Bind all items from the form to be added to the preferences table
            if (ModelState.IsValid) 
            {
                var prefs = _preferenceRepository.GetAllData(); // Get all preferences data
                foreach(var item in prefs) // Check if the student has already submitted a preference form
                {
                    if(item.student_id == submission.id) // If the user is found within the preferences table
                    {
                        TempData["denied"] = "You have already submitted your preferences"; // Inform user that they have already submitted preferences
                        return RedirectToAction("Index", "Home");
                    }
                } // Get each area associated with the submitted preference
                var area1 = _preferenceRepository.GetAreaByTitle(submission.pref1); 
                var area2 = _preferenceRepository.GetAreaByTitle(submission.pref2);
                var area3 = _preferenceRepository.GetAreaByTitle(submission.pref3);
                var area4 = _preferenceRepository.GetAreaByTitle(submission.pref4);
                var area5 = _preferenceRepository.GetAreaByTitle(submission.pref5);
                var area6 = _preferenceRepository.GetAreaByTitle(submission.pref6);
                List<int> areas = new List<int>()
                { area1.area_id, area2.area_id, area3.area_id, area4.area_id, area5.area_id, area6.area_id}; // Add preferred areas to list by area id
                List<string> supervisors = new List<string>()
                {submission.sup1, submission.sup2, submission.sup3, submission.sup4, submission.sup5, submission.sup6 }; // Add preferred supervisors to list
                bool isUnique = supervisors.Distinct().Count() == supervisors.Count();
                if (isUnique) // This is to validate the form and ensure students do not choose the same supervisor more than once
                {
                    int i = 1; // Set preference number to be iterated
                    foreach (var area in areas) // For each preferred area
                    {
                        Preference preferences = new Preference() // Create new Preference object
                        {
                            preference_id = i,
                            student_id = submission.id,
                            area_id = area,
                            time_submitted = DateTime.UtcNow


                        };

                        _preferenceRepository.Submit(preferences); // Submit the preference to the data through the repository
                        i++; // Increment i to assign next number to the following preference
                    }
                    TempData["success"] = "Success! FYP submission complete"; // Prepare a success message to ensure the user of task completion
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("All", "Cannot choose the same supervisor more than once, please re-enter"); // Inform user that supervisor can only be chosen once
                    ViewBag.supervisors = ViewBag.supervisors = new SelectList(sup, "supervisor_id", "name");
                    return View();
                }

               
                
            }
            else // If an error occurs during submission
            {
                ModelState.AddModelError("All", "Something went wrong, please try again");
                ViewBag.supervisors = ViewBag.supervisors = new SelectList(sup, "supervisor_id", "name");
                return View();
            }

        }

        [Authorize(Roles = "Supervisor")] // Only allow supervisors to access the area proposal form
        public IActionResult StaffForm()
        {
            try
            {
                var model = new AddArea(); // Using the 'AddArea' ViewModel
                var supervisor = _supervisorRepository.GetAllData();
                var user = User.Identity.Name;
                foreach(var item in supervisor) // Find the user logged in from the list of supervisors
                {
                    if(user == item.email)
                    {
                        model.name = item.name + " " + item.surname; // Automatically fil the supervisor name field
                        model.id = item.supervisor_id; // Automatically fill the supervisor id field
                    }
                }
                return View(model);
            }
            catch
            {
                return RedirectToAction("Error", "Home"); // If an  error occurs, redirect to home
            }       
        }

        [Authorize(Roles = "Supervisor")] // Only allows supervisors to utilise this feature 
        [HttpPost]
        public IActionResult StaffForm([Bind("area", "name", "id", "cosupname", "desc", "quota", "areakw", "reqres", "reqpre", "ethissues")] AddArea submission)
        { // Bind all the neccessary data submitted from the form
            // NOTE: Due to the nature of csv files, commas are used to define the shifting of a cell. Therefore to ensure data integrity, all commas have been replaced upon submission
            var area_title = submission.area.Replace(", ", " - "); 
            var area_keywords = submission.areakw.Replace(", ", " ");    
            var description_field = submission.desc.Replace(", ", " ")
                                                   .Replace("\r\n", string.Empty).Replace("\n", string.Empty)
                                                   .Replace("\r", string.Empty);
            string resources = submission.reqres.Replace(", ", " ");
            string prerequisites = submission.reqpre.Replace(", ", " ");
            string ethics = submission.ethissues.Replace(", ", " ");
            if (ModelState.IsValid) // If all data is submitted correctly 
            {
                Area area = new Area() // Add a new Area object
                {
                    supervisor_id = submission.id,
                    title = area_title,
                    description = description_field,
                    available = true,
                    area_quota = submission.quota,
                    keywords = area_keywords,
                    required_resources = resources,
                    required_prerequisites = prerequisites,
                    ethical_issues = ethics
                 };
                 _areaRepository.Submit(area); // Call repository to save new area to the database
                TempData["success"] = "Success! FYP Area Proposal was submitted"; // Prepare a success message to ensure the user of task completion
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("All", "Something went wrong, please try again"); // If an error occurs, inform the user.
                return View();
            }

        }

        public JsonResult GetArea(string supervisor_id) // This method is used to cascade dropdown list for areas and populate with those associated with the chosen supervisor
        {
            return Json(ar.Where(a => a.supervisor_id == supervisor_id));
        }
    }
}