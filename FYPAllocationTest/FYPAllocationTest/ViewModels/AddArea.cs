using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using FYPAllocationTest.Models;

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


        [Required(ErrorMessage = "Missing Area Description")]
        public string desc { get; set; }

        public int quota { get; set; }

        [Required(ErrorMessage = "Missing Area Keywords")]
        public string areakw { get; set; }
        [Required]
        public string reqres { get; set; }
        [Required]
        public string reqpre { get; set; }
        [Required]
        public string ethissues { get; set; }

        
    }
}
