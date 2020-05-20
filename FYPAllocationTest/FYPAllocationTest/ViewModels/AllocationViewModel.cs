using System;
using System.Collections.Generic;
using FYPAllocationTest.Models;
using System.Linq;
using System.Threading.Tasks;
namespace FYPAllocationTest.ViewModels
{
    public class AllocationViewModel
    {
        public IEnumerable<Student> student { get; set; }
        public IEnumerable<Supervisor> supervisor { get; set; }
        public IEnumerable<Area> area { get; set; }
        public IEnumerable<Preference> preferences { get; set; }
        public IEnumerable<Allocation> allocation { get; set; }

    }
}
