using BookBuddy.Business.Factories;
using BookBuddy.Business.Services.BooksPageService;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
 
    public class MyQuizzesPageController : PageControllerBase<MyQuizzesPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly ILogger<MyQuizzesPageController> _logger;

        public MyQuizzesPageController(IContentLoader contentLoader, ILogger<MyQuizzesPageController> logger)
        {
            _contentLoader = contentLoader;
            _logger = logger;
        }

        public IActionResult Index(MyQuizzesPage currentPage)
        {
            try
            {
                var siteSettingsReference = currentPage.SiteSettingsPage;

                SiteSettingsPage siteSettings = null!;
                if (siteSettingsReference != null)
                {
                    siteSettings = _contentLoader.Get<SiteSettingsPage>(siteSettingsReference);
                }
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
