using System.ComponentModel.DataAnnotations;

namespace BusShuttleDriver.Web.ViewModels
{
    public class DriverViewModel
    {
        public string? Id { get; set; } // Primary key
        [Display(Name = "First Name")]
        public string? Firstname { get; set; }
        [Display(Name = "Last Name")]
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        [Display(Name = "Status")]
        public bool IsActive { get; set; }
        public string? Role { get; set; }
    }
}
