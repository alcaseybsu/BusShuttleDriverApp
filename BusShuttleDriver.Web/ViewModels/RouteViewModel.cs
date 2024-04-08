using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusShuttleDriver.Web.ViewModels
{
    public class RouteViewModel
    {
        public int Id { get; set; }

        [Required]
        public int Order { get; set; }

        [Display(Name = "Bus")]
        public int SelectedBusId { get; set; }
        public required IEnumerable<SelectListItem> Buses { get; set; }

        [Display(Name = "Loop")]
        public int SelectedLoopId { get; set; }
        public required IEnumerable<SelectListItem> Loops { get; set; }
    }
}
