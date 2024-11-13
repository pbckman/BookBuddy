using BookBuddy.Business.Factories;
using BookBuddy.Business.Services.StartPageService;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
    public class StartPageController : PageControllerBase<StartPage>
    {
        private readonly StartPageService _startPageService;
        private readonly IContentLoader _contentLoader;
        private readonly StartPageFactory _startPageFactory;
        private readonly UrlResolver _urlResolver;

        public StartPageController(StartPageService startPageService, IContentLoader contentLoader, StartPageFactory startPageFactory, UrlResolver urlResolver)
        {
            _startPageService = startPageService;
            _contentLoader = contentLoader;
            _startPageFactory = startPageFactory;
            _urlResolver = urlResolver;
        }

        [Route("/")]
        public async Task<IActionResult> Index(StartPage currentPage)
        {
            SiteSettingsPage siteSettings = null!;

            if (User.IsInRole("CmsEditors") || User.IsInRole("CmsAdmins"))
            {
                var siteSettingsReference = currentPage.SiteSettingsPage;
                if (siteSettingsReference != null)
                {
                    siteSettings = _contentLoader.Get<SiteSettingsPage>(siteSettingsReference);
                }

                var heroSectionModel = _startPageFactory.CreateHeroSection(currentPage);

                var infoSectionModel = _startPageFactory.CreateInfoSection(currentPage);

                var books = await _startPageService.GetBooksAsync(currentPage);

                var model = new StartPageViewModel(currentPage, siteSettings)
                {
                    HeroSectionModel = heroSectionModel,
                    InfoSectionModel = infoSectionModel,
                    Books = books
                };

                return View(model);
            }
            else if (User.Identity.IsAuthenticated)
            {
                var searchPage = _contentLoader.GetChildren<BooksPage>(currentPage.ContentLink).FirstOrDefault();
                if (searchPage != null)
                {
                    return Redirect(_urlResolver.GetUrl(searchPage.ContentLink));
                }
            }
            else
            {
                var siteSettingsReference = currentPage.SiteSettingsPage;
                if (siteSettingsReference != null)
                {
                    siteSettings = _contentLoader.Get<SiteSettingsPage>(siteSettingsReference);
                }

                var heroSectionModel = _startPageFactory.CreateHeroSection(currentPage);

                var infoSectionModel = _startPageFactory.CreateInfoSection(currentPage);
                var books = await _startPageService.GetBooksAsync(currentPage);

                var model = new StartPageViewModel(currentPage, siteSettings)
                {
                    HeroSectionModel = heroSectionModel,
                    InfoSectionModel = infoSectionModel,
                    Books = books
                };

                return View(model);
            }

            return null!;
        }


    }
}
