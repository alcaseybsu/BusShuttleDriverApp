// handles user authentication and role assignment. 
//The SignUp method creates a new ApplicationUser object 
//with the user's username, first name, and last name. 
//It then calls the CreateAsync method of the UserManager 
//service to create the user in the database. 
//If the user is successfully created, the method checks if 
//the user is the first one to sign up and assigns them the Manager role. 
//For subsequent users, it assigns them the Driver role and sets them as 
//inactive for manager validation. Finally, the method redirects 
//the user to the login page or dashboard based on their role. 
//If there are any errors during the process, the method adds them 
//to the ModelState and redisplay the form.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using BusShuttleDriver.Web.ViewModels;

namespace BusShuttleDriver.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl ?? "/";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;

            _logger.LogInformation("User {Username} is attempting to log in.", model.Username); // Log before attempt

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username ?? "", model.Password ?? "", model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Username} logged in successfully.", model.Username);
                    return LocalRedirect(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User {Username} account locked out.", model.Username);
                    // Optionally, redirect to a lockout page or handle lockout
                    ModelState.AddModelError(string.Empty, "Account locked out.");
                }
                else
                {
                    _logger.LogWarning("Invalid login attempt for user {Username}.", model.Username);
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
