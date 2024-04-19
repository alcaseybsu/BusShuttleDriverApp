using System.ComponentModel.DataAnnotations;

namespace BusShuttleDriver.Web.ViewModels
{

    public class LoopViewModel
    {
        public int? Id { get; set; } // Primary key
        public string? Name { get; set; }
    }
}