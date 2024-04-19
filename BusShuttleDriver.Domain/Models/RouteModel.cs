using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusShuttleDriver.Domain.Models
{
    public class RouteModel
    {
        public int Id { get; set; } // Primary key
        [Display(Name = "Route Name")]
        public string? RouteName { get; set; } = string.Empty;
        public int BusId { get; set; } // Foreign key for Bus
        public int? LoopId { get; set; } // Foreign key for Loop
        public int Order { get; set; } // Sequence order of stops

        // Navigation properties
        public Bus Bus { get; set; } = new Bus(); // Initialize to avoid null reference
        public Loop Loop { get; set; } = new Loop(); // Initialize to avoid null reference

        public ICollection<Stop> Stops { get; set; } = new List<Stop>(); // Initialize to ensure it's never null
    }
}
