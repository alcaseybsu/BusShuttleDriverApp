using System.ComponentModel.DataAnnotations;

namespace BusShuttleDriver.Web.ViewModels
{
    public class LoopViewModel
    {
        [Key]
        public int? Id { get; set; } // Primary key

        public string? LoopName { get; set; }

        public int? StopsCount { get; set; }

        public bool HasActiveRoutes { get; set; }
        public ICollection<StopViewModel>? Stops { get; set; }
    }
}
