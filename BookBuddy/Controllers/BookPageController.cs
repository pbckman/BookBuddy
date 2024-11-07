using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
    public class BookPageController : PageControllerBase<BookPage>
    {
        private readonly IContentLoader _contentLoader;

        public BookPageController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }


        public IActionResult Index(BookPage currentPage)
        {
            var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);

            var siteSettingsPage = _contentLoader.Get<SiteSettingsPage>(startPage.SiteSettingsPage);

            var model = new BookPageViewModel(currentPage, siteSettingsPage);

            return View(model);
        }
    }
}
