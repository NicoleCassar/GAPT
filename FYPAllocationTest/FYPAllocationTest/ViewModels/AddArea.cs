using System.ComponentModel.DataAnnotations;

namespace FYPAllocationTest.ViewModels
{
    public class AddArea // ViewModel for area proposal form submission
    {   
        [Required(ErrorMessage = "Missing Area Title")]
        public string area { get; set; } // Title of area
        [Required]
        public string name { get; set; } // Supervisor name, being automatically filled
        [Required]
        public string id { get; set; } // Supervisor id, also automatically filled
        public string cosupname { get; set; } // Co-Supervisor name if necessary
        [Required(ErrorMessage = "Missing Area Description")]
        public string desc { get; set; } // Area description
        public int quota { get; set; } // Area quota
        [Required(ErrorMessage = "Missing Area Keywords")]
        public string areakw { get; set; } // Keywords to describe area
        [Required]
        public string reqres { get; set; } // Required resources
        [Required]
        public string reqpre { get; set; } // Required prerequisites
        [Required]
        public string ethissues { get; set; } // Possible ethical issues
    }
}
