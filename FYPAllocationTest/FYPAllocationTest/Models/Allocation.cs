using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public class Allocation
    {
        [Key]
        public int allocation_id { get; set; }
        public string student_id { get; set; }
        public string supervisor_id { get; set; }
    }
}
