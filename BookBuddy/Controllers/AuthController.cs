using BookBuddy.Business.Services.AccountService;
using BookBuddy.Business.Services.TranslationService;
using BookBuddy.Models.ViewModels;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;


namespace BookBuddy.Controllers
{
    public class AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AccountService accountService, AuthTranslationService translationService, ProfileService profileService) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly AccountService _accountService = accountService;
        private readonly AuthTranslationService _translationService = translationService;
        private readonly ProfileService _profileService = profileService;

        [HttpGet]
        public IActionResult SignUp(string lang)
        {
            var currentCulture = !string.IsNullOrEmpty(lang) ? lang : CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            ViewData["Title"] = _translationService.GetTranslation("signup", "title", currentCulture);
            ViewData["Description"] = _translationService.GetTranslation("signup", "description", currentCulture);
            ViewData["FirstnamePlaceholder"] = _translationService.GetTranslation("signup", "firstnamePlaceholder", currentCulture);
            ViewData["LastnamePlaceholder"] = _translationService.GetTranslation("signup", "lastnamePlaceholder", currentCulture);
            ViewData["EmailPlaceholder"] = _translationService.GetTranslation("signup", "emailPlaceholder", currentCulture);
            ViewData["PasswordPlaceholder"] = _translationService.GetTranslation("signup", "passwordPlaceholder", currentCulture);
            ViewData["ConfirmPasswordPlaceholder"] = _translationService.GetTranslation("signup", "confirmPasswordPlaceholder", currentCulture);
            ViewData["SignUpButton"] = _translationService.GetTranslation("signup", "signUpButton", currentCulture);
            ViewData["StatusMessage"] = "";
            ViewData["ErrorMessage"] = "";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model, string lang)
        {
            var currentCulture = !string.IsNullOrEmpty(lang) ? lang : CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = _translationService.GetTranslation("signup", "errorMessage", currentCulture);
                return RedirectToAction("SignUp", "Auth", new { currentCulture });
            }

            var (result, profile) = await _accountService.CreateUserAsync(model);

            if (result.Succeeded)
            {
                TempData["StatusMessage"] = _translationService.GetTranslation("signup", "statusMessage", currentCulture);
                return RedirectToAction("SignIn", "Auth", new { currentCulture });
            }

            TempData["ErrorMessage"] = _translationService.GetTranslation("signup", "errorMessage", currentCulture);
            return RedirectToAction("SignUp", "Auth", new { currentCulture });
        }


        [HttpGet]
        public IActionResult SignIn(string lang)
        {
            var currentCulture = !string.IsNullOrEmpty(lang) ? lang : CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            ViewData["Title"] = _translationService.GetTranslation("signin", "title", currentCulture);
            ViewData["Description"] = _translationService.GetTranslation("signin", "description", currentCulture);
            ViewData["EmailPlaceholder"] = _translationService.GetTranslation("signin", "emailPlaceholder", currentCulture);
            ViewData["PasswordPlaceholder"] = _translationService.GetTranslation("signin", "passwordPlaceholder", currentCulture);
            ViewData["RememberMeLabel"] = _translationService.GetTranslation("signin", "rememberMeLabel", currentCulture);
            ViewData["LoginButton"] = _translationService.GetTranslation("signin", "loginButton", currentCulture);
            ViewData["ErrorMessage"] = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string lang)
        {
            var currentCulture = !string.IsNullOrEmpty(lang) ? lang : CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var userId = user.Id;
                var profile = await _profileService.GetMainProfileAsync(userId);
                var profileId = profile.Id;

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(7),
                    HttpOnly = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.Lax
                };

                Response.Cookies.Append("SelectedProfileId", profileId.ToString(), cookieOptions);

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

            

            TempData["ErrorMessage"] = _translationService.GetTranslation("signin", "errorMessage", currentCulture);
            return RedirectToAction("SignIn", "Auth", new { currentCulture });
        }

        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            Response.Cookies.Delete("SelectedProfileId");
            Response.Cookies.Delete("SelectedSubProfileId");

            return RedirectToAction("Index", "StartPage");
        }
    }
}
