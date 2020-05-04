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

        public HomeController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        

        public IActionResult Index()
        {
            ViewBag.Title = "Student Data";
            var model = new StudentViewModel();
            model.student = _studentRepository.GetAllData();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
