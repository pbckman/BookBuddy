using BookBuddy.Business.Factories;
using BookBuddy.Business.Services.SiteSettingsService;
using BookBuddy.Business.Services.TranslationService;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BookBuddy.Controllers
{
    public class BookPageController : PageControllerBase<BookPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly ILogger<BookPageController> _logger;
        private readonly TranslationFactory _translationFactory;
        private readonly SiteSettingsService _siteSettingsService;

        public BookPageController(IContentLoader contentLoader, ILogger<BookPageController> logger, TranslationFactory translationFactory, SiteSettingsService siteSettingsService)
        {
            _contentLoader = contentLoader;
            _logger = logger;
            _translationFactory = translationFactory;
            _siteSettingsService = siteSettingsService;
        }

        //public IActionResult Index(BookPage currentPage)
        //{
        //    var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);

        //    var siteSettingsPage = _contentLoader.Get<SiteSettingsPage>(startPage.SiteSettingsPage);

        //    var model = new BookPageViewModel(currentPage, siteSettingsPage);

        //    return View(model);
        //}

        public IActionResult Index(BookPage currentPage)
        {
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
        }
        
    }
}
