using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusShuttleDriver.Domain.Models
{
    public class Loop
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<RouteModel> Routes { get; set; } = new List<RouteModel>(); // Collection to hold related routes
        public virtual ICollection<Stop> Stops { get; set; } = new List<Stop>(); // Collection to hold related stops   

    }
}

