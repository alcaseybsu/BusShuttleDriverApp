using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusShuttleDriver.Domain.Models
{
    public class RouteModel
    {
        public int Id { get; set; }
        public int Order { get; set; }

        // Navigation property for Loops
        public ICollection<Loop> Loops { get; set; } = new List<Loop>();
    }
}