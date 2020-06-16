using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPAllocationTest.Models
{
    public class Allocation // This is the model used to read and write content to the allocations table on the database
    {
        [Key] 
        public int allocation_id { get; set; } // Each allocation is given a unqiue id that is automatically incremented
        [ForeignKey("student")]
        public string student_id { get; set; } // A Foreign Key exists, as a means of referencing the student allocated by id
        public Student student { get; set; }
        [ForeignKey("supervisor")]
        public string supervisor_id { get; set; } // Supervisor id is also referenced through a Foreign Key
        public Supervisor supervisor { get; set; }
        public bool manual { get; set; } // This boolean value will identify whether or not a student has been manually allocated
    }
}
