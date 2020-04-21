using System;
using System.Collections.Generic;
using FYPAllocationTest.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.ViewModels
{
    public class StudentViewModel
    {
        public IEnumerable<Student> student { get; set; }
    }
}
