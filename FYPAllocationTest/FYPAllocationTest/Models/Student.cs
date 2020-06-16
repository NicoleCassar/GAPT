using System.ComponentModel.DataAnnotations;

namespace FYPAllocationTest.Models
{
    public class Student // Model class responsible for instantiating variables that reflect the database columns for table 'student' in the database
    {
        [Key]
        public string student_id { get; set; } // Student id number
        public string name { get; set; } // Student name
        public string surname { get; set; } // Student surname
        public string email { get; set; } // Student email address
        public double average_mark { get; set; } // Average mark of student
    }
}
