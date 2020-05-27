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
{
    public class FormsController : Controller
    {
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly IPreferenceRepository _preferenceRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private List<Supervisor> sup;
        private List<Area> ar;


        public FormsController(ISupervisorRepository supervisorRepository, IAreaRepository areaRepository, IPreferenceRepository preferenceRepository, IStudentRepository studentRepository, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _supervisorRepository = supervisorRepository;
            _areaRepository = areaRepository;
            _preferenceRepository = preferenceRepository;
            _studentRepository = studentRepository;
            _signinManager = signInManager;
            _userManager = userManager;
            var supervisors = _supervisorRepository.GetAllData().OrderByDescending(p => p.supervisor_id);
            var areas = _areaRepository.GetAllData().OrderByDescending(p => p.area_id);
            sup = new List<Supervisor>();
            ar = new List<Area>();
            foreach (var supervisor in supervisors)
            {
                int count = 0;
                foreach (var area in areas)
                {
                    if(supervisor.supervisor_id == area.supervisor_id)
                    {
                        count++;
                    }
                }
                if(count != 0)
                {
                    sup.Add
                    (
                        new Supervisor()
                        {
                            supervisor_id = supervisor.supervisor_id,
                            name = supervisor.name + " " + supervisor.surname
                        }
                    );
                }
                
            }
            foreach (var area in areas)
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
        [Authorize(Roles = "Student")]
        public IActionResult FormA()
        {
            try
            {
                var model = new AddStudent();
                ViewBag.supervisors = new SelectList(sup, "supervisor_id", "name");
                var student = _studentRepository.GetAllData();
                var user = User.Identity.Name;
                foreach (var item in student)
                {
                    if (user == item.email)
                    {
                        model.name = item.name + " " + item.surname;
                        model.id = item.student_id;
                    }
                }
                return View(model);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
            
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public IActionResult FormA([Bind("name", "id", "sup1", "pref1", "sup2", "pref2", "sup3", "pref3", "sup4", "pref4", "sup5", "pref5", "sup6", "pref6")] AddStudent submission)
        {
            
            if (ModelState.IsValid)
            {
                var prefs = _preferenceRepository.GetAllData();
                foreach(var item in prefs)
                {
                    if(item.student_id == submission.id)
                    {
                        TempData["denied"] = "You have already submitted your preferences";
                        return RedirectToAction("Index", "Home");
                    }
                }
                var area1 = _preferenceRepository.GetAreaByTitle(submission.pref1);
                var area2 = _preferenceRepository.GetAreaByTitle(submission.pref2);
                var area3 = _preferenceRepository.GetAreaByTitle(submission.pref3);
                var area4 = _preferenceRepository.GetAreaByTitle(submission.pref4);
                var area5 = _preferenceRepository.GetAreaByTitle(submission.pref5);
                var area6 = _preferenceRepository.GetAreaByTitle(submission.pref6);
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
                    TempData["success"] = "Success! FYP submission complete"; //Prepare a success message to ensure the user of task completion
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

        [Authorize(Roles = "Supervisor")]
        public IActionResult StaffForm()
        {
            try
            {
                var model = new AddArea();
                var supervisor = _supervisorRepository.GetAllData();
                var user = User.Identity.Name;
                foreach(var item in supervisor)
                {
                    if(user == item.email)
                    {
                        model.name = item.name + " " + item.surname;
                        model.id = item.supervisor_id;
                    }
                }
                return View(model);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
           
        }

        [Authorize(Roles = "Supervisor")]
        [HttpPost]
        public IActionResult StaffForm([Bind("area", "name", "id", "cosupname", "desc", "quota", "areakw", "reqres", "reqpre", "ethissues")] AddArea submission)
        {
            var area_title = submission.area.Replace(", ", " - ");
            var area_keywords = submission.areakw.Replace(", ", " ");    
            var description_field = submission.desc.Replace(", ", " ")
                                                   .Replace("\r\n", string.Empty).Replace("\n", string.Empty)
                                                   .Replace("\r", string.Empty);
            string resources = submission.reqres.Replace(", ", " ");
            string prerequisites = submission.reqpre.Replace(", ", " ");
            string ethics = submission.ethissues.Replace(", ", " ");
            if (ModelState.IsValid)
            {
                Area area = new Area()
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

                 _areaRepository.Submit(area);
                TempData["success"] = "Success! FYP Area Proposal was submitted"; //Prepare a success message to ensure the user of task completion
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("All", "Something went wrong, please try again");
                return View();
            }

        }

        public JsonResult GetArea(string supervisor_id)
        {
            return Json(ar.Where(a => a.supervisor_id == supervisor_id));
        }

        
    }
}