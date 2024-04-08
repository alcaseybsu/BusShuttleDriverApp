using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Web.ViewModels;
using BusShuttleDriver.Domain.Models;
using BusShuttleDriver.Data;


namespace BusShuttleDriver.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ManagerController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateDriver()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDriver(CreateDriverModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    UserName = model.Email, // Assuming email is used as username
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, model.Password ?? string.Empty);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Driver");
                    return RedirectToAction("Index"); // Assuming this action exists for listing drivers or showing success
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
                    driversViewModels.Add(new DriverViewModel
                    {
                        Id = user.Id,
                        Firstname = user.Firstname,
                        Lastname = user.Lastname,
                        Email = user.UserName, // Assuming the UserName property holds the email address
                        IsActive = user.IsActive,
                        Role = string.Join(", ", roles) // In case of multiple roles, join them. Otherwise, just display one.
                    });
                }
            }

            return View(driversViewModels);
        }


        public async Task<IActionResult> Activate(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found."); // Improved error handling
            }

            user.IsActive = true;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Driver activated successfully."; // Using TempData for success message
                return RedirectToAction("Index");
            }

            return View("Error"); // Generic error view
        }
    }
}

