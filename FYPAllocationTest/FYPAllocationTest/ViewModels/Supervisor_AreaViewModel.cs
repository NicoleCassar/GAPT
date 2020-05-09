using System;
using System.Collections.Generic;
using FYPAllocationTest.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.ViewModels
{
    public class Supervisor_AreaViewModel
    {
        public IEnumerable<Supervisor> supervisor { get; set; }
        public IEnumerable<Area> area { get; set; }


    }
}
