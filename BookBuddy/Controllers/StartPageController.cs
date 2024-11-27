using BookBuddy.Business.Services.StartPageService;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using EPiServer.Find.Framework;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
    public class StartPageController : PageControllerBase<StartPage>
    {
        private readonly StartPageService _startPageService;
        private readonly IContentLoader _contentLoader;
        private readonly StartPageFactory _startPageFactory;
        private readonly UrlResolver _urlResolver;
        private readonly IPageService _pageService;

        public StartPageController(StartPageService startPageService, IContentLoader contentLoader, StartPageFactory startPageFactory, UrlResolver urlResolver, IPageService pageService)
        {
            _startPageService = startPageService;
            _contentLoader = contentLoader;
            _startPageFactory = startPageFactory;
            _urlResolver = urlResolver;
            _pageService = pageService;
        }

        [Route("/")]
        public async Task<IActionResult> Index(StartPage currentPage, string lang)
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
                    Books = books,
                    SlideShowTitle = currentPage.SlideShowTitle,
                    SlideShowContent = currentPage.SlideShowContent,
                };

                return View(model);
            }
            else if (User.Identity.IsAuthenticated)
            {
                var booksPage = _pageService.GetBooksPage(lang);
                if (booksPage != null)
                {
                    var fallbackUrl = lang == "sv" ? "/sv/bocker/" : "/books/";
                    return Redirect(fallbackUrl);
                   
                }
                else
                {
                   
                    var booksPageUrl = _urlResolver.GetUrl(booksPage.ContentLink);
                    return Redirect(booksPageUrl);
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
                    Books = books,
                    SlideShowTitle = currentPage.SlideShowTitle,
                    SlideShowContent = currentPage.SlideShowContent,
                };

                return View(model);
            }

        }
    }
}
