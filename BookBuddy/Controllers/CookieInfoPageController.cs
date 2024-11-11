using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
    
    public class CookieInfoPageController : PageControllerBase<CookieInfoPage>
    {
        private readonly IContentLoader _contentLoader;

        public CookieInfoPageController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public IActionResult Index(CookieInfoPage currentPage)
        {
            var siteSettingsReference = currentPage.SiteSettingsPage;

            SiteSettingsPage siteSettings = null!;
            if (siteSettingsReference != null)
            {
                siteSettings = _contentLoader.Get<SiteSettingsPage>(siteSettingsReference);
            }

            var model = new CookieInformationPageViewModel(currentPage, siteSettings);
            return View(model);
        }
    }
}
