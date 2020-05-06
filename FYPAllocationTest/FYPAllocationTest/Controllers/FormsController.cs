using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYPAllocationTest.ViewModels;
using FYPAllocationTest.Models;

namespace FYPAllocationTest.Controllers
{
    public class FormsController : Controller
    {
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IAreaRepository _areaRepository;


        public FormsController(ISupervisorRepository supervisorRepository, IAreaRepository areaRepository)
        {
            _supervisorRepository = supervisorRepository;
            _areaRepository = areaRepository;

        }
        public IActionResult FormA()
        {
            var model = new Supervisor_AreaViewModel();
            model.supervisor = _supervisorRepository.GetAllData().OrderByDescending(p => p.supervisor_id);
            model.area = _areaRepository.GetAllData().OrderByDescending(p => p.area_id);
            return View(model);
        }

        public IActionResult StaffForm()
        {
            return View();
        }
    }
}