using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusShuttleDriver.Web.ViewModels
{
    public class RouteViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Route Name")]
        [Required]
        public string? RouteName { get; set; }
        [Required]
        public int Order { get; set; }
        [Display(Name = "Bus")]
        public int SelectedBusId { get; set; }
        public IEnumerable<SelectListItem> Buses { get; set; }
        [Display(Name = "Loop")]
        public int SelectedLoopId { get; set; }
        public IEnumerable<SelectListItem> Loops { get; set; }

        public RouteViewModel()
        {
            Buses = new List<SelectListItem>();
            Loops = new List<SelectListItem>();
        }
    }
}
