using System.ComponentModel.DataAnnotations;

namespace FYPAllocationTest.ViewModels
{
    public class AddStudent // ViewModel used for the submission of student preferences form
    {
        [Required]
        public string name { get; set; } // Student name, automatically filled
        [Required]
        public string id { get; set; } // Student id, also being automatially filled
        [Required(ErrorMessage = "Missing Supervisor for first preference")]
        public string sup1 { get; set; } // First choice of supervisor
        [Required(ErrorMessage = "Missing Area for first preference")]
        public string pref1 { get; set; } // First preference for area
        [Required(ErrorMessage = "Missing Supervisor for second preference")]
        public string sup2 { get; set; } // Second choice of supervisor
        [Required(ErrorMessage = "Missing Area for second preference")]
        public string pref2 { get; set; } // Second preference for area
        [Required(ErrorMessage = "Missing Supervisor for third preference")]
        public string sup3 { get; set; } // Third choice of supervisor
        [Required(ErrorMessage = "Missing Area for third preference")]
        public string pref3 { get; set; } // Third preference for area
        [Required(ErrorMessage = "Missing Area for fourth preference")]
        public string sup4 { get; set; } // Fourth choice of supervisor
        [Required(ErrorMessage = "Missing Area for fourth preference")]
        public string pref4 { get; set; } // Fourth preference for area
        [Required(ErrorMessage = "Missing Supervisor for fifth preference")]
        public string sup5 { get; set; } // Fifth choice of supervisor
        [Required(ErrorMessage = "Missing Area for fifth preference")]
        public string pref5 { get; set; } // Fifth preference for area
        [Required(ErrorMessage = "Missing Supervisor for sixth preference")]
        public string sup6 { get; set; } // Sixth choice of supervisor
        [Required(ErrorMessage = "Missing Area for sixth preference")]
        public string pref6 { get; set; } // Sixth preference for area

    }
}
