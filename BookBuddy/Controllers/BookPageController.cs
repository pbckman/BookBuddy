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

        public BookPageController(IContentLoader contentLoader, IAuthorizedService authorizedService)
        {
            _contentLoader = contentLoader;
            _authorizedService = authorizedService;
        }


        public IActionResult Index(BookPage currentPage)
        {
            if (!_authorizedService.IsUserAuthorizedAsync().Result)
                {
                    System.Diagnostics.Debug.WriteLine("User is not authorized");
                    return RedirectToAction("SignIn", "Auth");
                }

            var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);

            var siteSettingsPage = _contentLoader.Get<SiteSettingsPage>(startPage.SiteSettingsPage);

            var model = new BookPageViewModel(currentPage, siteSettingsPage);

            return View(model);
        }
    }
}
