namespace BusShuttleDriver.Web.ViewModels
{
    public class CreateDriverModel
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        // Default to false so a Mangager must activate
        public bool IsActive { get; set; } = false;

    }
}
