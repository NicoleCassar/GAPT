using System.Collections.Generic;
using FYPAllocationTest.Models;

namespace FYPAllocationTest.ViewModels
{
    public class Supervisor_AreaViewModel // ViewModel used to display Supervisor and Area details
    {
        public IEnumerable<Supervisor> supervisor { get; set; } // Allows for the retrieval of all supervisor data
        public IEnumerable<Area> area { get; set; } // Allows for the calling of all areas

    }
}
