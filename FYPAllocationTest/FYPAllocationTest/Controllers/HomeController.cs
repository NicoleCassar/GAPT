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

namespace FYPAllocationTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IAreaRepository _areaRepository;

        public HomeController(IStudentRepository studentRepository, ISupervisorRepository supervisorRepository, IAreaRepository areaRepository)
        {
            _studentRepository = studentRepository;
            _supervisorRepository = supervisorRepository;
            _areaRepository = areaRepository;
        }

        public IActionResult Index()
        {
            ViewBag.success = TempData["success"];
            var model = new StudentViewModel();
            model.student = _studentRepository.GetAllData();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Areas()
        {
            var model = new Supervisor_AreaViewModel();
            model.supervisor = _supervisorRepository.GetAllData().OrderByDescending(p => p.supervisor_id);
            model.area = _areaRepository.GetAllData().OrderByDescending(p => p.area_id);
            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }

        
    }
}

