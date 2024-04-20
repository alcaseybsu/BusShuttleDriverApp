using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BusShuttleDriver.Domain.Models
{
    public class Bus
    {
        public int BusId { get; set; }
        public int BusNumber { get; set; }
        public ICollection<RouteModel> Routes { get; set; } = new List<RouteModel>(); // Nav from Bus to Routes
    }

}
