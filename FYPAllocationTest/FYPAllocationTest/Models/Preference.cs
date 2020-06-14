using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPAllocationTest.Models
{
    public class Preference // Model for reading and writing data from the 'student_preference' table
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Do not allow Primary Key preference_id to auto imcrement
        public int preference_id { get; set; } // preference_id will be 1 through 6 for each students, as means of signifying preference order
        [ForeignKey("student")]
        public string student_id { get; set; } // Composite Key with student_id being a Foreign Key from the 'student' table
        public Student student { get; set; }
        [ForeignKey("supervisor_area")]
        public int area_id { get; set; } // Foreign Key to 'supervisor_area' table using area_id as a Foreign Key
        public Area supervisor_area { get; set; } 
        public DateTime time_submitted { get; set; } // Recorded time of submission
    }
}
