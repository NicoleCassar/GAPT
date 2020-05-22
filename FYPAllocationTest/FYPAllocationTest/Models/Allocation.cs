using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public class Allocation
    {
        [Key]
        public int allocation_id { get; set; }
        [ForeignKey("student")]
        public string student_id { get; set; }
        public Student student { get; set; }
        [ForeignKey("supervisor")]
        public string supervisor_id { get; set; }
        public Supervisor supervisor { get; set; }
        public bool manual { get; set; }
    }
}
