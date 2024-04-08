using System.ComponentModel.DataAnnotations;

namespace BusShuttleDriver.Web.ViewModels
{
    public class CreateDriverModel
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required.")]

        public string? Firstname { get; set; }

        [Display(Name = "Last Name")]
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
