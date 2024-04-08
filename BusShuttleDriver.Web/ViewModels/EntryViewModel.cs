using System.ComponentModel.DataAnnotations;
using NuGet.Packaging.Signing;

namespace BusShuttleDriver.Web.ViewModels
{
    public class EntryViewModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int NumBoarded { get; set; }
        public int NumLeft { get; set; }

    }
}


