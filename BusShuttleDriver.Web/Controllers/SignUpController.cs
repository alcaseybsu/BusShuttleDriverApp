using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using BusShuttleDriver.Web.ViewModels;
using Microsoft.EntityFrameworkCore; // Ensure this namespace matches where your SignUpViewModel is located

namespace BusShuttleDriver.Web.Controllers
{
    public class SignUpController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SignUpController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View(); // Returns the empty form view for a new user sign-up
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username, // Use the built-in UserName property
                    Firstname = model.Firstname,
                    Lastname = model.Lastname
                };

                if (model.Password != null)
                {
                    user.Email = model.Password;
                }

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Check if the roles exist before assigning them
                    if (!await _roleManager.RoleExistsAsync("Manager"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Manager"));
                    }
                    if (!await _roleManager.RoleExistsAsync("Driver"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Driver"));
                    }

                    var isFirstUser = !await _userManager.Users.AnyAsync();
                    var roleAssignmentResult = isFirstUser
                        ? await _userManager.AddToRoleAsync(user, "Manager")
                        : await _userManager.AddToRoleAsync(user, "Driver");

                    // Optionally, sign in the user immediately after registration
                    // await _signInManager.SignInAsync(user, isPersistent: false);

                    // Redirect the user to the appropriate page after successful sign-up
                    return RedirectToAction("Index", "Home");
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
