using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoListInfrastructure.Models.ViewModels.Account;

namespace ToDoList.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signingManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this._signingManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            var newAccount = new IdentityUser()
            {
                Email = register.Email,
                UserName = register.Email
            };

            var result = await this._userManager.CreateAsync(newAccount, register.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors.Select(x => x.Description))
                {
                    ModelState.AddModelError("", error);
                }

                return View();
            }

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var result = await this._signingManager.PasswordSignInAsync(login.Email, login.Password, true, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Login Error");
                return View();
            }

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await this._signingManager.SignOutAsync();

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(returnUrl);
        }
    }
}
