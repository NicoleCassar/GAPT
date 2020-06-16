using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FYPAllocationTest.Models;
using FYPAllocationTest.ViewModels;

namespace FYPAllocationTest.Controllers
{ // This controller is in charge of all the View included within the Home View folder
    public class HomeController : Controller
    { // Setting global variables for controller
        private readonly IStudentRepository _studentRepository;
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IAreaRepository _areaRepository;

        public HomeController(IStudentRepository studentRepository, ISupervisorRepository supervisorRepository, IAreaRepository areaRepository)
        { // HomeController constuctor
            _studentRepository = studentRepository;
            _supervisorRepository = supervisorRepository;
            _areaRepository = areaRepository;
        }

        public IActionResult Index() // Loading Home page
        {
            ViewBag.Denied = TempData["denied"]; // If a student has already submitted their preferences.
            ViewBag.success = TempData["success"]; // On successful completion of an operation
            return View();
        }

        public IActionResult Areas() // Loading list of areas page
        {
            var model = new Supervisor_AreaViewModel(); // Using 'Supervisor_AreaViewModel'
            model.supervisor = _supervisorRepository.GetAllData().OrderByDescending(p => p.supervisor_id); // Get all supervisor data
            model.area = _areaRepository.GetAllData(); // get all area data
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int id) // Details page for a selected area
        {
            var area = _areaRepository.GetAreaById(id); 
            if (area == null) // Ensure area still exists
                return RedirectToAction("Areas", "Home"); // If it no longer exists, refresh the page. This is mainly done as a validation measure as an area that has been deleted will not show up in the list
            else
                return View(area);
        }

        public IActionResult Error() // Custom 404 page, mainly used for page not found or access denied
        {
            return View();
        }

        
    }
}

