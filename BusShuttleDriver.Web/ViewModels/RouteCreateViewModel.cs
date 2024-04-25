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

        [Required(ErrorMessage = "Enter a route name.")]
        [Display(Name = "Route Name")]
        public string RouteName { get; set; } = string.Empty;

        public SelectList? AvailableLoops { get; set; }

        public List<SelectListItem> AvailableStops { get; set; }

        //public SelectList? AvailableStops { get; set; }

        // IDs of stops in order in route
        public List<int> OrderedStopIds { get; set; } = new List<int>();

        // Details of stops displayed on RouteCreate view
        public List<StopViewModel>? Stops { get; set; }
    }
}
