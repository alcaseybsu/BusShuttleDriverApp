using System;
using System.ComponentModel.DataAnnotations;
using BusShuttleDriver.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusShuttleDriver.Web.ViewModels
{
    public class EntryViewModel
    {
        [Required]
        public int SessionId { get; set; }

        public string? LoopName { get; set; }

        [Display(Name = "Stop")]
        [Required(ErrorMessage = "Please select a stop.")]
        public int SelectedStopId { get; set; }

        [Display(Name = "Stop")]
        [Required(ErrorMessage = "Please select a stop.")]
        public string? StopName { get; set; }

        [Required(ErrorMessage = "Number of passengers boarded is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Number must be non-negative.")]
        public int Boarded { get; set; }

        [Required(ErrorMessage = "Number of passengers left behind is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Number must be non-negative.")]
        public int LeftBehind { get; set; }

        public DateTime Timestamp { get; set; }

        [Display(Name = "Select a Stop")]
        public SelectList AvailableStops { get; set; } = new SelectList(Array.Empty<object>());

        public EntryViewModel()
        {
            Timestamp = DateTime.UtcNow; // Initialize timestamp
        }
    }
}
