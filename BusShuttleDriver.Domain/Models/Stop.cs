using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusShuttleDriver.Domain.Models
{
    public class Stop
    {
        public int Id { get; set; }
        public string? Location { get; set; }
    }
}