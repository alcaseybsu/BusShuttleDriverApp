using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Domain.Models;
using Microsoft.EntityFrameworkCore;

public class RouteStop
{
    [Key]
    public int RouteId { get; set; }
    public Route? Route { get; set; }

    public int StopId { get; set; }
    public Stop? Stop { get; set; }

    public int Order { get; set; }
}
