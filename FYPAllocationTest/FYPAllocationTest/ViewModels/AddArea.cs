using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.ViewModels
{
    public class AddArea
    {
        [Required]
        public string area { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string id { get; set; }

        public string cosupname { get; set; }

        [Required(ErrorMessage = "Missing Area Keywords")]
        public string areakw { get; set; }

        [Required(ErrorMessage = "Missing Area Description")]
        public string desc { get; set; }

        public string reqres { get; set; }

        public string reqpre { get; set; }

        public string ethissues { get; set; }

        public int quota { get; set; }
    }
}
