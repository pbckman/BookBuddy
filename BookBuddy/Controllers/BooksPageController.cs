using System;
using System.Globalization;
using System.Linq;
using BookBuddy.Business.Factories;
using BookBuddy.Business.Services.BooksPageService;
using BookBuddy.Business.Services.CategoryService;
using BookBuddy.Business.Services.SiteSettingsService;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Authorization;
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
        private readonly SiteSettingsService _siteSettingsService;
        private readonly TranslationFactory _translationFactory;

        public BooksPageController(IContentLoader contentLoader, IBooksPageService booksPageService, UrlResolver urlResolver, ILogger<BooksPageController> logger, ICategoryService categoryService, SiteSettingsService siteSettingsService, TranslationFactory translationFactory)
        {
            _contentLoader = contentLoader;
            _booksPageService = booksPageService;
            _urlResolver = urlResolver;
            _logger = logger;
            _categoryService = categoryService;
            _siteSettingsService = siteSettingsService;
            _translationFactory = translationFactory;
        }

        public IActionResult Index(BooksPage currentPage, ContentReference content)
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

        // [HttpGet]
        // public async Task<IActionResult> Search(BooksPage currentPage, [FromQuery] string query)
        // {
        //     try
        //     {
        //         var siteSettingsReference = currentPage.SiteSettingsPage;
        //         var currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        //         SiteSettingsPage siteSettings = null;
        //         if (siteSettingsReference != null)
        //         {
        //             siteSettings = _contentLoader.Get<SiteSettingsPage>(siteSettingsReference);
        //         }

        //         var allCategories = _categoryService.GetAllCategories(currentCulture);
        //         var searchResult = await _booksPageService.SearchAsync(query, currentPage.Language);
        //         var bookPages = searchResult.Items.Select(item => item).ToList();
        //         var bookPageModels = bookPages.Select(bookpage => BookPageFactory.CreateBookPageModel(bookpage, _urlResolver, allCategories)).ToList();
        //         var filteredCategories = allCategories.Where(category => bookPageModels.Any(book => book.Categories.Any(cat => cat.Value == category.Value)));
        //         var model = new BooksPageViewModel(currentPage, siteSettings)
        //         {
        //             Query = query,
        //             Result = bookPageModels,
        //             Categories = filteredCategories.ToList()
        //         };
        //         return View("Index", model);
                
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError($"ERROR : BooksPageController.Search() : {ex.Message}");
        //         return View("Error");
        //     }
        // }
        [HttpGet]
        public async Task<IActionResult> Search(BooksPage currentPage, [FromQuery] string query, [FromQuery] string category)
        {
            try
            {
                var fileName = "BooksPage.xml";
                var translations = _translationFactory.GetTranslationsForView(fileName, "bookspage", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
                ViewData["Translations"] = translations;
                
                var currentCulture = CultureInfo.CurrentCulture;
                var siteSettings = _siteSettingsService.GetSiteSettings(currentPage.SiteSettingsPage);

                var allUsedCategories = await _categoryService.GetAllUsedCategories(query, currentCulture, currentPage);
                var bookPageModels = await _booksPageService.GetFilteredBookPages(query, currentPage, category, allUsedCategories);
                var model = BookPageFactory.CreateBooksPageViewModel(currentPage, siteSettings, query, bookPageModels, allUsedCategories);

                string url = string.Empty;
                if (model.CurrentPage?.ContentLink != null)
                {
                    url = Url.Action("ChangeLanguage", "Language", new { contentReference = model.CurrentPage.ContentLink.ID, language = "en" });
                    System.Diagnostics.Debug.WriteLine("URL: " + url);
                }

                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR : BooksPageController.Search() : {ex.Message}");
            }
                return View("Error");
        }
    }
}
