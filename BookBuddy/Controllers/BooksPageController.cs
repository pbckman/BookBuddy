using System;
using System.Linq;
using BookBuddy.Business.Factories;
using BookBuddy.Business.Services.BooksPageService;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
    public class BooksPageController : PageControllerBase<BooksPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly IBooksPageService _booksPageService;
        private readonly UrlResolver _urlResolver;

        public BooksPageController(IContentLoader contentLoader, IBooksPageService booksPageService, UrlResolver urlResolver)
        {
            _contentLoader = contentLoader;
            _booksPageService = booksPageService;
            _urlResolver = urlResolver;
        }

        public IActionResult Index(BooksPage currentPage)
        {
            var siteSettingsReference = currentPage.SiteSettingsPage;

            SiteSettingsPage siteSettings = null;
            if (siteSettingsReference != null)
            {
                siteSettings = _contentLoader.Get<SiteSettingsPage>(siteSettingsReference);
            }
            var model = new BooksPageViewModel(currentPage, siteSettings);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Search(BooksPage currentPage, [FromQuery] string query)
        {
            var siteSettingsReference = currentPage.SiteSettingsPage;

            SiteSettingsPage siteSettings = null;
            if (siteSettingsReference != null)
            {
                siteSettings = _contentLoader.Get<SiteSettingsPage>(siteSettingsReference);
            }

            var searchResult = await _booksPageService.SearchAsync(query, currentPage.Language);
            var bookPages = searchResult.Items.Select(item => item).ToList();
            var bookPageModels = bookPages.Select(bookpage => BookPageFactory.CreateBookPageModel(bookpage, _urlResolver)).ToList();
            var model = new BooksPageViewModel(currentPage, siteSettings)
            {
                Query = query,
                Result = bookPageModels
            };
            return View("Index", model);
        }
    }
}
