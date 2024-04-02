using System.ComponentModel.DataAnnotations;

namespace BusShuttleDriver.Web.ViewModels
{
    public class SignUpViewModel
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public required string? Password { get; set; }
        public bool IsActive { get; set; } = false;
    }
}