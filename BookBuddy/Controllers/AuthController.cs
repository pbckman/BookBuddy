using BookBuddy.Business.Services.AccountService;
using BookBuddy.Business.Services.TranslationService;
using BookBuddy.Models.ViewModels;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace BookBuddy.Controllers
{
    public class AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AccountService accountService, AuthTranslationService translationService) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly AccountService _accountService = accountService;
        private readonly AuthTranslationService _translationService = translationService;

        [Route("{lang}/auth/signup")]
        public IActionResult SignUp(string lang = "en")
        {
            ViewData["Title"] = _translationService.GetTranslation("signup", "title", lang);
            ViewData["Description"] = _translationService.GetTranslation("signup", "description", lang);
            ViewData["FirstnamePlaceholder"] = _translationService.GetTranslation("signup", "firstnamePlaceholder", lang);
            ViewData["LastnamePlaceholder"] = _translationService.GetTranslation("signup", "lastnamePlaceholder", lang);
            ViewData["EmailPlaceholder"] = _translationService.GetTranslation("signup", "emailPlaceholder", lang);
            ViewData["PasswordPlaceholder"] = _translationService.GetTranslation("signup", "passwordPlaceholder", lang);
            ViewData["ConfirmPasswordPlaceholder"] = _translationService.GetTranslation("signup", "confirmPasswordPlaceholder", lang);
            ViewData["SignUpButton"] = _translationService.GetTranslation("signup", "signUpButton", lang);
            ViewData["StatusMessage"] = "";

            return View();
        }

        [HttpPost]
        [Route("{lang}/auth/signup")]
        public async Task<IActionResult> SignUp(SignUpViewModel model, string lang = "en")
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
                
            var (result, profile) = await _accountService.CreateUserAsync(model);

            if (result.Succeeded)
            {
                return RedirectToAction("SignIn", "Auth", new { lang });
            }

            ViewData["StatusMessage"] = "Something went wrong, please try again later.";
            return View(model);
        }


        [Route("{lang}/auth/signin")]
        public IActionResult SignIn(string lang = "en")
        {
            ViewData["Title"] = _translationService.GetTranslation("signin", "title", lang);
            ViewData["Description"] = _translationService.GetTranslation("signin", "description", lang);
            ViewData["EmailPlaceholder"] = _translationService.GetTranslation("signin", "emailPlaceholder", lang);
            ViewData["PasswordPlaceholder"] = _translationService.GetTranslation("signin", "passwordPlaceholder", lang);
            ViewData["RememberMeLabel"] = _translationService.GetTranslation("signin", "rememberMeLabel", lang);
            ViewData["LoginButton"] = _translationService.GetTranslation("signin", "loginButton", lang);
            return View();
        }

        [HttpPost]
        [Route("{lang}/auth/signin")]
        public async Task<IActionResult> SignIn(SignInViewModel model, string lang = "en")
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        await _signInManager.RefreshSignInAsync(user);
                        return RedirectToAction("Index", "StartPage");
                    }
                }
            }

            ViewData["StatusMessage"] = _translationService.GetTranslation("signin", "errorMessage", lang);

            return View(model);
        }

        [HttpPost]
        [Route("/")]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            Response.Cookies.Delete("SelectedProfileId");

            return RedirectToAction("Index", "StartPage");
        }
    }
}
