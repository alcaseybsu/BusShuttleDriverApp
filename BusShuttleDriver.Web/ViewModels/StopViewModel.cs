using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusShuttleDriver.Web.ViewModels
{
    public class StopViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }
        public int? Order { get; set; }

        [ForeignKey("Loop")]
        public int? SelectedLoopId { get; set; } = 0;
        public string? LoopName { get; set; }
        public SelectList? AvailableLoops { get; set; }

        public IEnumerable<StopViewModel> StopsList { get; set; } = new List<StopViewModel>();
    }
}
