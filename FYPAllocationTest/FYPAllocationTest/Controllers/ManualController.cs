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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYPAllocationTest.Controllers
{
    public class ManualController : Controller
    {
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly IAllocationRepository _allocationsRepository;
        private readonly IStudentRepository _studentRepository;
        private List<Student> stud;
        private List<Supervisor> sup;
        private List<Area> ar;
        private List<Allocation> alloc;

        public ManualController(ISupervisorRepository supervisorRepository, IAreaRepository areaRepository, IAllocationRepository allocationsRepository, IStudentRepository studentRepository)
        {
            _supervisorRepository = supervisorRepository;
            _areaRepository = areaRepository;
            _allocationsRepository = allocationsRepository;
            _studentRepository = studentRepository;
            var students = _studentRepository.GetAllData().OrderByDescending(p => p.student_id);
            var supervisors = _supervisorRepository.GetAllData().OrderByDescending(p => p.supervisor_id);
            var areas = _areaRepository.GetAllData().OrderByDescending(p => p.area_id);
            var allocations = _allocationsRepository.GetAllData().OrderByDescending(p => p.supervisor_id);
            stud = new List<Student>();
            sup = new List<Supervisor>();
            ar = new List<Area>();
            alloc = new List<Allocation>();
            foreach (var student in students)
            {
                bool exists = false;
                foreach (var allocation in allocations)
                {
                    if(allocation.student_id == student.student_id)
                    {
                        exists = true;
                    }
                }
                if (!exists)
                {
                    stud.Add
                    (
                        new Student()
                        {
                            student_id = student.student_id,
                            name = student.name + " " + student.surname
                        }
                    );
                }

            }
            foreach (var supervisor in supervisors)
            {
                int total = 0;
                foreach(var area in areas)
                {
                    if(area.supervisor_id == supervisor.supervisor_id)
                    {
                        total = total + area.area_quota;
                    }
                }
                if(supervisor.quota != 0 && total == 0)
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
        }
        public IActionResult ManualAlloc()
        {
            var model = new AddAllocation();
            ViewBag.students = new SelectList(stud, "student_id", "name");
            ViewBag.supervisors = new SelectList(sup, "supervisor_id", "name");
            return View(model);
        }

        [HttpPost]
        public IActionResult Manual([Bind("student", "supervisor")] AddAllocation addAllocation)
        {
            if (ModelState.IsValid)
            {
                Allocation allocation = new Allocation()
                {
                    student_id = addAllocation.student,
                    supervisor_id = addAllocation.supervisor,
                    manual = true

                };
                _allocationsRepository.Create(allocation);
                Supervisor_Update(addAllocation.supervisor);
                TempData["Success"] = "Success! Student was allocated, see below";
                return RedirectToAction("FYPAlloc", "Allocation");
            }
            else
            {
                ModelState.AddModelError("All", "Missing options, please Select all options");
                return View();
            }
        }

        public void Supervisor_Update(string id)
        {
            var current_supervisor = _supervisorRepository.GetSupervisorById(id);
            Supervisor new_quota = new Supervisor()
            {
                supervisor_id = current_supervisor.supervisor_id,
                quota = current_supervisor.quota-1
            };
            _supervisorRepository.UpdateQuota(new_quota);
        }
    }
}
