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
the user to the login page or dashboard based on their role. 
If there are any errors during the process, the method adds them 
to the ModelState and redisplay the form.
*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using BusShuttleDriver.Web.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;


namespace BusShuttleDriver.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username ?? throw new InvalidOperationException("Email cannot be empty."),
                    Email = model.Username,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    IsActive = false
                };

                if (model.Password is null)
                {
                    ModelState.AddModelError(string.Empty, "Password cannot be null.");
                    return View(model);
                }
                var result = await _userManager.CreateAsync(user, model.Password!);


                if (result.Succeeded)
                {
                    var isFirstUser = (await _userManager.Users.CountAsync()) == 1;

                    string roleName = isFirstUser ? "Manager" : "Driver";
                    user.IsActive = isFirstUser;

                    var roleExists = await _roleManager.RoleExistsAsync(roleName);
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(roleName));
                    }

                    await _userManager.AddToRoleAsync(user, roleName);

                    if (isFirstUser)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
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
                if (model.Username is null || model.Password is null)
                {
                    ModelState.AddModelError(string.Empty, "Email or Password cannot be empty.");
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Email} logged in successfully.", model.Username);
                    return LocalRedirect(returnUrl ?? "/");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }
    }
}