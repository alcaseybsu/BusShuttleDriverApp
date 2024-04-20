using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Domain.Models;

public class RouteSession
{
    public int Id { get; set; }
    public int BusId { get; set; }
    public Bus? Bus { get; set; }
    public int LoopId { get; set; }
    public Loop? Loop { get; set; }
    public DateTime StartTime { get; set; }
    public int DriverId { get; set; }  // Track which driver is logged in
    public Driver? Driver { get; set; }
    public bool IsActive { get; set; } = true;  // Indicates if route currently active
}
