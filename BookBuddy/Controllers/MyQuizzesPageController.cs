using BookBuddy.Business.Factories;
using BookBuddy.Business.Services.AccountService;
using BookBuddy.Business.Services.AuthorizedService;
using BookBuddy.Business.Services.BooksPageService;
using BookBuddy.Business.Services.QuizResultService;
using BookBuddy.Business.Services.SiteSettingsService;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
 
    public class MyQuizzesPageController : PageControllerBase<MyQuizzesPage>
    {
        private readonly SiteSettingsService _siteSettingsService;
        private readonly ILogger<MyQuizzesPageController> _logger;
        private readonly IAuthorizedService _authorizedService;

        public MyQuizzesPageController(SiteSettingsService siteSettingsService, ILogger<MyQuizzesPageController> logger, IAuthorizedService authorizedService)
        {
            _siteSettingsService = siteSettingsService;
            _logger = logger;
            _authorizedService = authorizedService;
        }

        public async Task<IActionResult> Index(MyQuizzesPage currentPage)
        {
            try
            {
                if (!_authorizedService.IsUserAuthorizedAsync().Result)
                {
                    System.Diagnostics.Debug.WriteLine("User is not authorized");
                    return RedirectToAction("Index", "StartPage");
                }

                var siteSettings = _siteSettingsService.GetSiteSettings(currentPage.SiteSettingsPage);
                var model = new MyQuizzesPageViewModel(currentPage, siteSettings);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR :  MyQuizzesPageController.Index() : {ex.Message}");
                return View("Error");
            }
        }
    }
}
