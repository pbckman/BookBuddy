using BookBuddy.Business.Factories;
using BookBuddy.Business.Services.AccountService;
using BookBuddy.Business.Services.BooksPageService;
using BookBuddy.Business.Services.QuizResultService;
using BookBuddy.Business.Services.SiteSettingsService;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
 
    public class MyQuizzesPageController : PageControllerBase<MyQuizzesPage>
    {
        private readonly SiteSettingsService _siteSettingsService;
        private readonly ILogger<MyQuizzesPageController> _logger;


        public MyQuizzesPageController(SiteSettingsService siteSettingsService, ILogger<MyQuizzesPageController> logger)
        {
            _siteSettingsService = siteSettingsService;
            _logger = logger;

        }

        public async Task<IActionResult> Index(MyQuizzesPage currentPage)
        {
            try
            {
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
