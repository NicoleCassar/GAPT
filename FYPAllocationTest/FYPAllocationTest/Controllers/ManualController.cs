using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FYPAllocationTest.ViewModels;
using FYPAllocationTest.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace FYPAllocationTest.Controllers
{ // This controller serves the purpose of controlling processes relating to manual allocations
    public class ManualController : Controller
    { // Setting the global variables for the controller
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly IAllocationRepository _allocationsRepository;
        private readonly IStudentRepository _studentRepository;
        private List<Student> stud;
        private List<Supervisor> sup;
        private List<Area> ar;
        private List<Allocation> alloc;

        public ManualController(ISupervisorRepository supervisorRepository, IAreaRepository areaRepository, IAllocationRepository allocationsRepository, IStudentRepository studentRepository)
        { // This constructor will create lists of data to be used as dropdown options
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
            foreach (var student in students) // Go through each student
            {
                bool exists = false; // Used to check if a student has been allocated
                foreach (var allocation in allocations) // Go through alocations
                {
                    if(allocation.student_id == student.student_id) // If the student is found to have been allocated
                    {
                        exists = true; 
                    }
                }
                if (!exists) // If student has not yet been allocated
                {
                    stud.Add // Add student to list of unallocated students
                    (
                        new Student()
                        {
                            student_id = student.student_id,
                            name = student.name + " " + student.surname
                        }
                    );
                }

            }
            foreach (var supervisor in supervisors) // Find supervisor who are still available
            {
                int total = 0; // Use counter to check if a supervisor has any avaiable areas
                foreach(var area in areas) // Go through areas
                {
                    if(area.supervisor_id == supervisor.supervisor_id) // Add up a total of all area quotas
                    {
                        total = total + area.area_quota;
                    }
                }
                if(supervisor.quota != 0 && total == 0) // Since manual allocation will only include supervisors for consideration, only include supervisors bot opting for an area quota
                {
                    sup.Add // Adding supervisor object to list of supervisors still available
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

        [Authorize(Roles = "Administrator")] // Restrict manual allocation to administrators
        public IActionResult ManualAlloc() // This method will load the manual allocations page
        {
            var model = new AddAllocation();
            ViewBag.students = new SelectList(stud, "student_id", "name"); // Set up unallocated students dropdown list
            ViewBag.supervisors = new SelectList(sup, "supervisor_id", "name"); // Set up available supervisors dropdown list
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Manual([Bind("student", "supervisor")] AddAllocation addAllocation) // Manually allocating a student to a supervisor
        {
            if (ModelState.IsValid)
            {
                Allocation allocation = new Allocation() // Creare new object of type Allocation
                {
                    student_id = addAllocation.student,
                    supervisor_id = addAllocation.supervisor,
                    manual = true

                };
                _allocationsRepository.Create(allocation); // Call repository to add a new allocation
                Supervisor_Update(addAllocation.supervisor); // Update supervisor quota
                TempData["Success"] = "Success! Student was allocated, see below"; // Inform administrator of success
                return RedirectToAction("Allocation", "Allocation");
            }
            else
            {
                ModelState.AddModelError("All", "Missing options, please Select all options"); // If an option is not selected upon submission
                return View();
            }
        }

        public void Supervisor_Update(string id) // Updates supervisor quota
        {
            var current_supervisor = _supervisorRepository.GetSupervisorById(id); // Get supervisor to be updated
            Supervisor new_quota = new Supervisor()
            {
                supervisor_id = current_supervisor.supervisor_id,
                quota = current_supervisor.quota-1
            };
            _supervisorRepository.UpdateQuota(new_quota); // Send new details to repository to update supervisor quota
        }
    }
}
