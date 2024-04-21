/*
handles user authentication and role assignment.
The SignUp method creates a new ApplicationUser object
with the user's username, first name, and last name.
It then calls the CreateAsync method of the UserManager
service to create the user in the database.
If the user is successfully created, the method checks if
the user is the first one to sign up and assigns them the Manager role.
For subsequent users, it assigns them the Driver role and sets them as
inactive for manager validation. Finally, the method redirects
the user to the login page or dashboard (manager dashboard page) based on their role.
If there are any errors during the process, the method adds them
to the ModelState and redisplays the form.
*/
using System;
using System.Threading.Tasks;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using BusShuttleDriver.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusShuttleDriver.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager,Driver")]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Username,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                IsActive = false
            };

            var password =
                model.Password ?? throw new InvalidOperationException("Password cannot be null.");
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            // Assign role
            var isFirstUser = (await _userManager.Users.CountAsync()) == 1;
            var role = isFirstUser ? "Manager" : "Driver";
            user.IsActive = isFirstUser;

            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            await _userManager.AddToRoleAsync(user, role);
            await _userManager.UpdateAsync(user);
            await _signInManager.SignInAsync(user, isPersistent: false);

            return isFirstUser
                ? RedirectToAction("Dashboard", "Manager")
                : RedirectToAction("Start", "Entry");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl ?? "/";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl ?? "/";

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null || !user.IsActive)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false
                );

                if (result.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    if (userRoles.Contains("Manager"))
                    {
                        return RedirectToAction("Dashboard", "Manager");
                    }
                    else if (userRoles.Contains("Driver"))
                    {
                        return RedirectToAction("Start", "Entry");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
