using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusShuttleDriver.Domain.Models
{
    public class Entry
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int NumBoarded { get; set; }
        public int NumLeft { get; set; }
    }
}