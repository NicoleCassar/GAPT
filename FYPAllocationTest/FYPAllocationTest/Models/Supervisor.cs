using System.ComponentModel.DataAnnotations;

namespace FYPAllocationTest.Models
{
    public class Supervisor // Model for 'supervisors' table, containing all variables found as columns on the 'supervisor' table
    {
        [Key]
        public string supervisor_id { get; set; } // Id pf supervisor
        public string name { get; set; } // Name of supervisor
        public string surname { get; set; } // Surname of supervisor
        public string email { get; set; } // Email address of supervisor
        public int quota { get; set; } // Supervisor total quota

    }
}
