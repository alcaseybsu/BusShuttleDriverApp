using System.ComponentModel.DataAnnotations;

namespace BusShuttleDriver.Web.ViewModels
{
    public class CreateDriverModel
    {
        [Required(ErrorMessage = "First name is required.")]
        public string? Firstname { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        public string? Lastname { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

        // Default to false so a Mangager must activate
        public bool IsActive { get; set; } = false;

    }
}
