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
        public string? RouteName { get; set; } = string.Empty; // Initialize with empty string if default value is acceptable

        public int Order { get; set; } // Sequence order of stops

        public int BusId { get; set; } // Foreign key for Bus
        public Bus Bus { get; set; } = new Bus(); // Initialize to avoid null reference

        public int LoopId { get; set; } // Foreign key for Loop
        public Loop Loop { get; set; } = new Loop(); // Initialize to avoid null reference

        public ICollection<Stop> Stops { get; set; } = new List<Stop>(); // Initialize to ensure it's never null
    }
}
