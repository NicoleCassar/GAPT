using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYPAllocationTest.ViewModels;
using FYPAllocationTest.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FYPAllocationTest.Controllers
{
    public class FormsController : Controller
    {
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IAreaRepository _areaRepository;
        private List<Supervisor> sup;
        private List<Area> ar;


        public FormsController(ISupervisorRepository supervisorRepository, IAreaRepository areaRepository)
        {
            _supervisorRepository = supervisorRepository;
            _areaRepository = areaRepository;
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
            var model = new Supervisor_AreaViewModel();
            model.supervisor = _supervisorRepository.GetAllData().OrderByDescending(p => p.supervisor_id);
            model.area = _areaRepository.GetAllData().OrderByDescending(p => p.area_id);
            ViewBag.supervisors = new SelectList(sup, "supervisor_id", "name");
            return View(model);
        }

        public IActionResult StaffForm()
        {
            return View();
        }

        public JsonResult GetArea(string supervisor_id)
        {
            return Json(ar.Where(a => a.supervisor_id == supervisor_id));
        }

        
    }
}