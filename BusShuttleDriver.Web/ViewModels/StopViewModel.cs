using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BusShuttleDriver.Web.ViewModels
{
    public class StopViewModel
    {
        public int Id { get; set; } // Primary key
        public string? Name { get; set; }
        public double Latitude { get; set; }
        public int Order { get; set; }
        public double Longitude { get; set; }
        public int SelectedLoopId { get; set; }
        public string? SelectedLoopName { get; set; }
        public List<SelectListItem> AvailableLoops { get; set; } = new List<SelectListItem>();
    }
}
