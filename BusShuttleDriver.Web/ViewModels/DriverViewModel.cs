using System.ComponentModel.DataAnnotations;

namespace BusShuttleDriver.Web.ViewModels
{
    public class DriverViewModel
    {
        public string? Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public string? Role { get; set; }
    }
}
