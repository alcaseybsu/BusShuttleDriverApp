using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusShuttleDriver.Web.ViewModels
{
    public class StopViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Stop Name")]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "Latitude")]
        public double Latitude { get; set; }

        [Required]
        [Display(Name = "Longitude")]
        public double Longitude { get; set; }

        [Required]
        [Display(Name = "Loop")]
        public int SelectedLoopId { get; set; }

        // This property is used to populate a dropdown list of available loops.
        public IEnumerable<SelectListItem> AvailableLoops { get; set; } = new List<SelectListItem>();

        // Additional fields, validation added as needed.
    }
}
