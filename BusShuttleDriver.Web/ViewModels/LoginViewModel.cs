using System.ComponentModel.DataAnnotations;

namespace BusShuttleDriver.Web.ViewModels
{

    public class LoginViewModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }

}
