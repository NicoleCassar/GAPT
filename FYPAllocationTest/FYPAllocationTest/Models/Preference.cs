using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public class Preference
    {
        [Key]
        public int preference_id { get; set; }
        public string student_id { get; set; }
        public int area_id { get; set; }
        public DateTime time_submitted { get; set; }
    }
}
