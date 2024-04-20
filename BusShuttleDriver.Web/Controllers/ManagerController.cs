using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using BusShuttleDriver.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusShuttleDriver.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ManagerController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context
        )
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = new ManagerDashboardViewModel
            {
                TotalBuses = await _context.Buses.CountAsync(),
                TotalRoutes = await _context.Routes.CountAsync(),
                TotalLoops = await _context.Loops.CountAsync(),
                TotalStops = await _context.Stops.CountAsync(),
                TotalEntries = await _context.Entries.CountAsync(),
                // Add method to calculate TotalReports if needed
                TotalReports = 0 // Placeholder
            };

            return View("Dashboard", model); // Make sure to create Dashboard.cshtml under Views/Manager
        }

        public async Task<IActionResult> ActivateDrivers()
        {
            var drivers = await _userManager
                .Users.Where(user => _userManager.IsInRoleAsync(user, "Driver").Result)
                .Select(user => new DriverViewModel
                {
                    Id = user.Id,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    Role = "Driver"
                })
                .ToListAsync();

            return View("ActivateDrivers", drivers); // Make sure to create ActivateDrivers.cshtml under Views/Manager
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public IActionResult CreateDriver()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDriver(CreateDriverModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, model.Password ?? string.Empty);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Driver");
                    return RedirectToAction("Index", "Manager");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var driverRoleName = "Driver";
            var driversViewModels = new List<DriverViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(driverRoleName))
                {
                    driversViewModels.Add(
                        new DriverViewModel
                        {
                            Id = user.Id,
                            Firstname = user.Firstname,
                            Lastname = user.Lastname,
                            Email = user.UserName,
                            IsActive = user.IsActive,
                            Role = string.Join(", ", roles) // In multiple roles, join them. Else, display one.
                        }
                    );
                }
            }

            return View(driversViewModels);
        }

        public async Task<IActionResult> ToggleActiveStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            user.IsActive = !user.IsActive; // Toggle active status
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            // Handle errors if update failed
            return View("Error");
        }
    }
}
