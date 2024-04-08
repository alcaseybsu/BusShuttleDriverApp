using System.ComponentModel.DataAnnotations;

namespace BusShuttleDriver.Web.ViewModels
{
    public class SignUpViewModel
    {
        [Display(Name = "First Name")]
        public string? Firstname { get; set; }
        [Display(Name = "Last Name")]
        public string? Lastname { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        public string? Username { get; set; } // Email

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
