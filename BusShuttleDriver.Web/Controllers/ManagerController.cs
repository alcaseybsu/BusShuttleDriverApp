using Microsoft.AspNetCore.Authorization;
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
                    UserName = model.Username,
                    IsActive = true
                };

                if (model.Password is null)
                {
                    ModelState.AddModelError("Password", "Password is required");
                    return View(model);
                }

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Driver");
                    return RedirectToAction(nameof(Index));
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
            var drivers = await _userManager.GetUsersInRoleAsync("Driver");
            var inactiveDrivers = drivers.Where(d => !d.IsActive).ToList();
            return View(inactiveDrivers);
        }

        public async Task<IActionResult> Activate(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsActive = true;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    // Show a success message

                    // Redirect back to the list or to a success page
                    return RedirectToAction(nameof(Index));
                }
            }
            return View("Error"); // Or handle errors as needed
        }
    }
}

