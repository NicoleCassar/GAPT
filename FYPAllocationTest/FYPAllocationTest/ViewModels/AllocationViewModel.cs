using System.Collections.Generic;
using FYPAllocationTest.Models;
namespace FYPAllocationTest.ViewModels
{
    public class AllocationViewModel // ViewModel used when prsenting perfom allocation page to administrators
    {
        public IEnumerable<Student> student { get; set; } // Allows for the loading of all students
        public IEnumerable<Supervisor> supervisor { get; set; } // Enables the retrieving of supervisors
        public IEnumerable<Area> area { get; set; } // Allows for a call to be made for all areas
        public IEnumerable<Preference> preferences { get; set; } // Enables the use of all preference
        public IEnumerable<Allocation> allocation { get; set; } // Allows for the display of all allocations
    }
}
