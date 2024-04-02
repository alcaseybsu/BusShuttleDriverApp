using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BusShuttleDriver.Domain.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Username { get; set; }

        //how to store password securely?
        public string? Password { get; set; }

    }
}
