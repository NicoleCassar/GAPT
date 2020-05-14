using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.ViewModels
{
    public class Imports
    {
        public IFormFile studentimport { get; set; }
        public IFormFile supervisorimport { get; set; }

    
    }
}
