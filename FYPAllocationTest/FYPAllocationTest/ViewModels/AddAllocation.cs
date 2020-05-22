using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.ViewModels
{
    public class AddAllocation
    {
        [Required(ErrorMessage ="Please choose a student to assign")]
        public string student{ get; set; }
        [Required(ErrorMessage = "Please choose a supervisor to assign")]
        public string supervisor { get; set; }

    }
}
