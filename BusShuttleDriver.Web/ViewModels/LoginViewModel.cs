using System.ComponentModel.DataAnnotations;

namespace BusShuttleDriver.Web.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        public string? Username { get; set; } // Email

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
