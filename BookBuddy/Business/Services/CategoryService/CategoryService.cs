using System;
using System.Globalization;
using BookBuddy.Business.Factories;
using BookBuddy.Business.Services.BooksPageService;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.DDS;
using BookBuddy.Models.Pages;
using EPiServer.Data.Dynamic;
using EPiServer.Web.Routing;

namespace BookBuddy.Business.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly ILogger<CategoryService> _logger;
    private readonly IBooksPageService _booksPageService;
    private readonly UrlResolver _urlResolver;


    public CategoryService(ILogger<CategoryService> logger, IBooksPageService booksPageService, UrlResolver urlResolver)
    {
        _logger = logger;
        _booksPageService = booksPageService;
        _urlResolver = urlResolver;
    }

    public List<CategoryModel> GetAllCategories(CultureInfo currentCulture)
    {
        try
        {
            var store = DynamicDataStoreFactory.Instance.GetStore(typeof(CategoryItem));
            var allCategories = store.Items<CategoryItem>().Where(category => category.Language == currentCulture.TwoLetterISOLanguageName).ToList();
            if (allCategories.Count > 0)
            {
                return allCategories.Select(category => new CategoryModel
                {
                    Text = category.Text,
                    Value = category.Value
                }).ToList();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : CategoryService.GetAllCategories() : {ex.Message}");
        }
        return [];
    }

    public async Task<List<CategoryModel>> GetAllUsedCategories(string query, CultureInfo currentCulture, BooksPage currentPage)
    {
        try
        {
            var allCategories = GetAllCategories(currentCulture);
            // var allBooks = await _booksPageService.GetAllAsync(currentCulture);
            // var allBooks = await _booksPageService.SearchAsync(query, currentCulture, currentPage);
            
            // var allBooks = await _booksPageService.GetAllAsync(currentPage.Language);
            // var bookPages = allBooks.Items.Select(item => item).ToList();
            // var bookPageModels = bookPages.Select(bookpage => BookPageFactory.CreateBookPageModel(bookpage, _urlResolver, allCategories)).ToList();
            var bookPageModels = await _booksPageService.GetAllBookPages(currentPage, allCategories);

            // var filteredCategories = allUsedCategories.Where(category => bookPageModels.Any(book => book.Categories.Any(cat => cat.Value == category.Value)));
            var filteredCategories = allCategories.Where(category => bookPageModels.Any(book => book.Categories.Any(cat => cat.Value == category.Value)));
            return filteredCategories.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : CategoryService.GetAllUsedCategories() : {ex.Message}");
        }
        return [];
    }

}
