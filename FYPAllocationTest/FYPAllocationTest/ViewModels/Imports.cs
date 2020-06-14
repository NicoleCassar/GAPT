using Microsoft.AspNetCore.Http;

namespace FYPAllocationTest.ViewModels
{
    public class Imports // ViewModel to allow import of csv files
    {
        public IFormFile studentimport { get; set; } // Accept upload of student as csv file
        public IFormFile supervisorimport { get; set; } // Accept upload of supervisor list as csv file
    }
}
