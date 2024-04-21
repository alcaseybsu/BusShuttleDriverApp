using BusShuttleDriver.Domain.Models;

namespace BusShuttleDriver.Web.ViewModels
{
    public class BusCreateViewModel
    {
        public Bus NewBus { get; set; } = new Bus();
        public IEnumerable<Bus>? ExistingBuses { get; set; } = new List<Bus>();
    }
}
