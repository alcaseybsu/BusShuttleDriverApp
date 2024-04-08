using Microsoft.AspNetCore.Identity;

namespace BusShuttleDriver.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public bool IsActive { get; set; }
    }
}