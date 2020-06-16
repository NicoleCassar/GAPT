using System.ComponentModel.DataAnnotations;

namespace FYPAllocationTest.ViewModels
{
    public class LoginViewModel // ViewModel for login page
    {
        [Required]
        [Display(Name = "Email address")] // Email address field, also serving as username in this case
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } // Password field
    }
}
