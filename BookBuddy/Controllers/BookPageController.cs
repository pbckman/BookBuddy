using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
    public class BookPageController : PageController<BookPage>
    {
        private readonly IContentLoader _contentLoader;

        public BookPageController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public IActionResult Index(BookPage currentPage)
        {
            var siteSettingsReference = currentPage.SiteSettingsPage;

            SiteSettingsPage siteSettings = null;
            if (siteSettingsReference != null)
            {
                siteSettings = _contentLoader.Get<SiteSettingsPage>(siteSettingsReference);
            }
            var model = new BookPageViewModel(currentPage, siteSettings);
            return View(model);
        }
    }
}
