using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusShuttleDriver.Web.ViewModels
{
    public class RouteEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Route Name")]
        public string? LoopName { get; set; }

        [Required]
        [Display(Name = "Select Loop")]
        public int SelectedLoopId { get; set; }

        public SelectList? AvailableLoops { get; set; }

        public List<StopInfo> Stops { get; set; }

        // This could be used for re-ordering or selecting/deselecting stops from the route.
        public List<int> SelectedStopIds { get; set; }

        public RouteEditViewModel()
        {
            Stops = new List<StopInfo>();
            SelectedStopIds = new List<int>();
        }

        // Nested class to represent stop details
        public class StopInfo
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public int Order { get; set; } // If ordering of stops within the route is needed
        }
    }
}
