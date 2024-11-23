using BookBuddy.Business.Services.AccountService;
using BookBuddy.Business.Services.TranslationService;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;


namespace BookBuddy.Controllers
{
    public class AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AccountService accountService, AuthTranslationService translationService) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly AccountService _accountService = accountService;
        private readonly AuthTranslationService _translationService = translationService;

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

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        // Kontrollera om användaren har rollen "WebAdmin"
                        if (await _userManager.IsInRoleAsync(user, "WebAdmin"))
                        {
                            // Om användaren är en "WebAdmin", omdirigera till startsidan för admins
                            return RedirectToAction("AdminDashboard", "Admin");
                        }
                        else
                        {

                            var booksPage = _contentLoader.GetChildren<BooksPage>(ContentReference.StartPage).FirstOrDefault();
                            if (booksPage != null)
                            {
                                var booksPageUrl = _urlResolver.GetUrl(booksPage.ContentLink);
                                return Redirect(booksPageUrl);
                            }
                            else
                            {
                                return View("Error");
                            }
                        }
                    }


                }

                //if (user != null)
                //{
                //    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                //    if (result.Succeeded)
                //    {
                //        await _signInManager.RefreshSignInAsync(user);
                //        //return RedirectToAction("Index", "StartPage");

                //        if (User.Identity.IsAuthenticated)
                //        {
                //            Console.WriteLine("User is authenticated after signing in.");

                //            var booksPage = _contentLoader.GetChildren<BooksPage>(ContentReference.StartPage).FirstOrDefault();
                //            if (booksPage != null)
                //            {
                //                var booksPageUrl = _urlResolver.GetUrl(booksPage.ContentLink);
                //                Console.WriteLine($"Redirecting to BooksPage URL: {booksPageUrl}");
                //                return Redirect(booksPageUrl);
                //            }
                //            else
                //            {
                //                Console.WriteLine("BooksPage not found.");
                //                return View("Error");
                //            }
                //        }
                //        else
                //        {
                //            Console.WriteLine("User is not authenticated after signing in.");
                //            return View("Error");
                //        }

                //    }
                //}
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
