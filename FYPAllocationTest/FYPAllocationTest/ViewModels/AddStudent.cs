using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.ViewModels
{
    public class AddStudent
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string id { get; set; }

        [Required(ErrorMessage = "Missing Supervisor for first preference")]
        public string sup1 { get; set; }
        [Required(ErrorMessage = "Missing Area for first preference")]
        public string pref1 { get; set; }
        [Required(ErrorMessage = "Missing Supervisor for second preference")]
        public string sup2 { get; set; }
        [Required(ErrorMessage = "Missing Area for second preference")]
        public string pref2 { get; set; }
        [Required(ErrorMessage = "Missing Supervisor for third preference")]
        public string sup3 { get; set; }
        [Required(ErrorMessage = "Missing Area for third preference")]
        public string pref3 { get; set; }
        public string sup4 { get; set; }
        [Required(ErrorMessage = "Missing Area for fourth preference")]
        public string pref4 { get; set; }
        [Required(ErrorMessage = "Missing Supervisor for fifth preference")]
        public string sup5 { get; set; }
        [Required(ErrorMessage = "Missing Area for fifth preference")]
        public string pref5 { get; set; }
        [Required(ErrorMessage = "Missing Supervisor for sixth preference")]
        public string sup6 { get; set; }
        [Required(ErrorMessage = "Missing Area for sixth preference")]
        public string pref6 { get; set; }
    }
}
