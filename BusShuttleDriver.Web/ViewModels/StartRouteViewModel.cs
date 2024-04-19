using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusShuttleDriver.Web.ViewModels
{
    public class StartRouteViewModel
    {
        public int SelectedBusId { get; set; }
        public int SelectedLoopId { get; set; }
        public List<SelectListItem> AvailableBuses { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableLoops { get; set; } = new List<SelectListItem>();
    }
}