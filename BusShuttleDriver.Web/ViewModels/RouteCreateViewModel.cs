using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusShuttleDriver.Web.ViewModels
{
    public class RouteCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Select a loop.")]
        [Display(Name = "Select Loop")]
        public int SelectedLoopId { get; set; }

        public SelectList AvailableLoops { get; set; }

        public SelectList AvailableStops { get; set; }

        // IDs of stops in the order they should appear in route
        public List<int> OrderedStopIds { get; set; } = new List<int>();

        // Optional: Details of stops to be displayed on RouteCreate view
        public List<StopViewModel>? Stops { get; set; }
    }
}
