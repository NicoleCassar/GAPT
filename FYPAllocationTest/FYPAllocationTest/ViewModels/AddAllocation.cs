using System.ComponentModel.DataAnnotations;

namespace FYPAllocationTest.ViewModels
{
    public class AddAllocation // ViewModel for adding allocations manually
    {
        [Required(ErrorMessage ="Please choose a student to assign")]
        public string student{ get; set; } // Choose a student to add for allocation
        [Required(ErrorMessage = "Please choose a supervisor to assign")]
        public string supervisor { get; set; } // Chose a supervisor to add for allocation

    }
}
