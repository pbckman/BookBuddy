using BookBuddy.Business.Services.AuthorizedService;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ResultModels;
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
        private readonly ProfileService _profileService;
        private readonly IQuizResultService _quizResultService;

        public BookPageController(IContentLoader contentLoader, IAuthorizedService authorizedService, ILogger<BookPageController> logger, TranslationFactory translationFactory, SiteSettingsService siteSettingsService, ProfileService profileService, IQuizResultService quizResultService)
        {
            _contentLoader = contentLoader;
            _authorizedService = authorizedService;
            _logger = logger;
            _translationFactory = translationFactory;
            _siteSettingsService = siteSettingsService;
            _profileService = profileService;
            _quizResultService = quizResultService;
        }

        public async Task<IActionResult> Index(BookPage currentPage)
        {
            if (!_authorizedService.IsUserAuthorizedAsync().Result)
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

                var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage, new LanguageSelector("sv"));

                var siteSettings = _contentLoader.Get<SiteSettingsPage>(startPage.SiteSettingsPage);

                var model = new BookPageViewModel(currentPage, siteSettings);

                var currentUrl = HttpContext.Request.Path.Value;
                var quizUrl = $"{currentUrl}quiz";
                ViewData["QuizUrl"] = quizUrl;

                var profile = await _profileService.GetSelectedProfileAsync();
                if(profile != null)
                {
                    var quiz = _contentLoader.GetChildren<QuizPage>(currentPage.ContentLink, new LanguageSelector(currentPage.Language.Name)).FirstOrDefault();
                    if (quiz != null)
                    {
                        var quizStatus = await _quizResultService.GetResultStatusAsync(profile.Id, quiz.ContentLink.ID);
                        model.Status = quizStatus;
                    }
                    else
                        model.Status = ResultStatus.None;
                }
                else
                    model.Status = ResultStatus.None;
                

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
