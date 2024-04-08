using BusShuttleDriver.Data;
using BusShuttleDriver.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Username, Firstname = model.Firstname, Lastname = model.Lastname };
                var result = await _userManager.CreateAsync(user, model.Password ?? "");

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("Manager"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Manager"));
                    }
                    if (!await _roleManager.RoleExistsAsync("Driver"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Driver"));
                    }

                    var isFirstUser = !await _userManager.Users.AnyAsync();
                    if (isFirstUser)
                    {
                        await _userManager.AddToRoleAsync(user, "Manager");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Driver");
                    }

                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
    }
}