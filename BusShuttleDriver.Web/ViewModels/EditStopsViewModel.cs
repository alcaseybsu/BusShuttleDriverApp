using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusShuttleDriver.Web.ViewModels
{
    public class EditStopsViewModel
    {
        public int RouteId { get; set; } // Route Id
        public string? RouteName { get; set; } // Route name (for display)
        public List<StopViewModel> Stops { get; set; } // List of stops to be reordered

        public EditStopsViewModel()
        {
            RouteName = string.Empty; // Initialize to ensure not null
            Stops = new List<StopViewModel>(); // Initialize to ensure not null
        }
    }
}