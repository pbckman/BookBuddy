using System;
using System.Globalization;
using System.Linq;
using BookBuddy.Business.Factories;
using BookBuddy.Business.Services.BooksPageService;
using BookBuddy.Business.Services.CategoryService;
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
        private readonly ILogger<BooksPageController> _logger;
        private readonly ICategoryService _categoryService;

        public BooksPageController(IContentLoader contentLoader, IBooksPageService booksPageService, UrlResolver urlResolver, ILogger<BooksPageController> logger, ICategoryService categoryService)
        {
            _contentLoader = contentLoader;
            _booksPageService = booksPageService;
            _urlResolver = urlResolver;
            _logger = logger;
            _categoryService = categoryService;
        }

        public IActionResult Index(BooksPage currentPage)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError($"ERROR : BooksPageController.Index() : {ex.Message}");
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(BooksPage currentPage, [FromQuery] string query)
        {
            try
            {
                var siteSettingsReference = currentPage.SiteSettingsPage;
                var currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

                SiteSettingsPage siteSettings = null;
                if (siteSettingsReference != null)
                {
                    siteSettings = _contentLoader.Get<SiteSettingsPage>(siteSettingsReference);
                }

                var searchResult = await _booksPageService.SearchAsync(query, currentPage.Language);
                var bookPages = searchResult.Items.Select(item => item).ToList();
                var bookPageModels = bookPages.Select(bookpage => BookPageFactory.CreateBookPageModel(bookpage, _urlResolver)).ToList();
                var allCategories = _categoryService.GetAllCategories(currentCulture);
                var filteredCategories = allCategories.Where(category => bookPageModels.Any(book => book.Categories.Contains(category.Value))).ToList();
                var model = new BooksPageViewModel(currentPage, siteSettings)
                {
                    Query = query,
                    Result = bookPageModels,
                    Categories = filteredCategories
                };
                return View("Index", model);
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR : BooksPageController.Search() : {ex.Message}");
                return View("Error");
            }
        }
    }
}
