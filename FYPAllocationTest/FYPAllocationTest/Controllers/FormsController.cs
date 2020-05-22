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
                        supervisor_id = item.supervisor_id, 
                        name = item.name + " " + item.surname
                    }
                );
            }
            foreach (var item in areas)
            {
                ar.Add
                (
                    new Area()
                    {
                        supervisor_id = item.supervisor_id, 
                        title = item.title
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

        [HttpPost]
        public IActionResult FormA([Bind("name", "id", "sup1", "pref1", "sup2", "pref2", "sup3", "pref3", "sup4", "pref4", "sup5", "pref5", "sup6", "pref6")] AddStudent submission)
        {
            
            if (ModelState.IsValid)
            {
                
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

        public IActionResult StaffForm()
        {
            var model = new AddArea();
            return View(model);
        }

        [HttpPost]
        public IActionResult StaffForm([Bind("area", "name", "id", "cosupname", "areakw", "desc", "reqres", "reqpre", "ethissues", "quota")] AddArea submission)
        {
            var area_title = submission.area.Replace(", ", " - ");
            var area_keywords = submission.areakw.Replace(", ", " ");
            var description_field = submission.desc.Replace(", ", " ");
            string resources;
            string prerequisites;
            string ethics;
            if(submission.reqres != null)
                resources = submission.reqres.Replace(", ", " ");
            if (submission.reqpre != null)
                prerequisites = submission.reqpre.Replace(", ", " ");
            if (submission.ethissues != null)
                ethics = submission.ethissues.Replace(", ", " ");
            if (ModelState.IsValid)
            {
                Area area = new Area()
                {
                    supervisor_id = submission.id,
                    title = area_title,
                    description = description_field,
                    available = true,
                    area_quota = submission.quota
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