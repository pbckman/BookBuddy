using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using BootstrapBlazor.Components;
using EPiServer.DataAbstraction.Internal;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
    public class StartPageController : PageControllerBase<StartPage>
    {
        private readonly IContentLoader _contentLoader;

        public StartPageController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        //[Route("/")]
        //public IActionResult Index(StartPage currentPage)
        //{
        //    var siteSettingsReference = currentPage.SiteSettingsPage;

        //    SiteSettingsPage siteSettings = null;
        //    if (siteSettingsReference != null)
        //    {
        //        siteSettings = _contentLoader.Get<SiteSettingsPage>(siteSettingsReference);
        //    }

        //    var model = new StartPageViewModel(currentPage, siteSettings);

        //    return View(model);
        //}

        [Route("/")]
        public IActionResult Index(StartPage currentPage)
        {
            if (User.Identity.IsAuthenticated)
            {
                var searchPage = _contentLoader.GetChildren<BooksPage>(currentPage.ContentLink)
                                               .FirstOrDefault();

                if (searchPage != null)
                {
                    return Redirect(UrlResolver.Current.GetUrl(searchPage.ContentLink));
                }
            }
            else
            {
                var siteSettingsReference = currentPage.SiteSettingsPage;

                SiteSettingsPage siteSettings = null!;
                if(siteSettingsReference != null)
                {
                    siteSettings = _contentLoader.Get<SiteSettingsPage>(siteSettingsReference); 
                }
               
                var model = new StartPageViewModel(currentPage, siteSettings);
                return View(model);
            }

            return null!;
        }
    }
}
