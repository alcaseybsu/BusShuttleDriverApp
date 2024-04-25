using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusShuttleDriver.Domain.Models
{
    public class IntervalReport
    {
        public string? Interval { get; set; }
        public int Boarded { get; set; }
    }
}
