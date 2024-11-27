using BookBuddy.Business.Services.AuthorizedService;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
    public class BookPageController : PageControllerBase<BookPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly IAuthorizedService _authorizedService;
        private readonly ILogger<BookPageController> _logger;
        private readonly TranslationFactory _translationFactory;
        private readonly SiteSettingsService _siteSettingsService;

        public BookPageController(IContentLoader contentLoader, IAuthorizedService authorizedService, ILogger<BookPageController> logger, TranslationFactory translationFactory, SiteSettingsService siteSettingsService)
        {
            _contentLoader = contentLoader;
            _authorizedService = authorizedService;
            _logger = logger;
            _translationFactory = translationFactory;
            _siteSettingsService = siteSettingsService;
        }

        public IActionResult Index(BookPage currentPage)
        {
            if(!_authorizedService.IsUserAuthorizedAsync().Result)
               {
                  System.Diagnostics.Debug.WriteLine("User is not authorized");
                  return RedirectToAction("SignIn", "Auth");
               }

            try
            {
                var fileName = "BookCardBig.xml";
                var translation = _translationFactory.GetTranslationsForView(fileName, "bookpage", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
                ViewData["Translation"] = translation;

                var currentCulture = CultureInfo.CurrentCulture;

                var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);

                var siteSettings = _contentLoader.Get<SiteSettingsPage>(startPage.SiteSettingsPage);

                var model = new BookPageViewModel(currentPage, siteSettings);

                var currentUrl = HttpContext.Request.Path.Value;
                var quizUrl = $"{currentUrl}quiz";
                ViewData["QuizUrl"] = quizUrl;


                return View(model);

            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR : BookPageController.Search() : {ex.Message}");
                return View("Error");
            }


            //var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);

            //var siteSettingsPage = _contentLoader.Get<SiteSettingsPage>(startPage.SiteSettingsPage);

            //var model = new BookPageViewModel(currentPage, siteSettingsPage);

            //return View(model);
        }
    }
}
