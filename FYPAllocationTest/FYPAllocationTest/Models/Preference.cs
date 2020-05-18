using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public class Preference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int preference_id { get; set; }
        [ForeignKey("student")]
        public string student_id { get; set; }
        public Student student { get; set; }
        [ForeignKey("supervisor_area")]
        public int area_id { get; set; }
        public Area supervisor_area { get; set; }
        public DateTime time_submitted { get; set; }
    }
}
