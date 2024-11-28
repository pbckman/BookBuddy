using System;
using System.Globalization;
using BookBuddy.Business.Factories;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.Pages;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Find.Framework.Statistics;
using EPiServer.Find.UnifiedSearch;
using EPiServer.Web.Routing;

namespace BookBuddy.Business.Services.BooksPageService;

public class BooksPageService : IBooksPageService
{
    private readonly ILogger<BooksPageService> _logger;
    private readonly UrlResolver _urlResolver;

    public BooksPageService(ILogger<BooksPageService> logger, UrlResolver urlResolver)
    {
        _logger = logger;
        _urlResolver = urlResolver;
    }

    public Task<IContentResult<BookPage>> SearchAsync(string query, CultureInfo culture)
    {
        try
        {
            var searchClient = SearchClient.Instance;
            var language = new Language(
                culture.Name,
                "default",
                culture.TwoLetterISOLanguageName,
                "porter",
                null,
                null,
                null
            );

            // var results = searchClient.Search<BookPage>()
            //     .For(query)
            //     .Filter(x => x.Language.Name.Match(culture.Name))
            //     .GetContentResult();

            var results = Task.Run(() => searchClient.Search<BookPage>()
                .For(query)
                .Filter(x => x.Language.Name.Match(culture.Name))
                .Take(16)
                .GetContentResult());

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR :  BooksPageService.SearchAsync() : {ex.Message}");
            return Task.FromResult<IContentResult<BookPage>>(null!);   
        }
    }
    
    public Task<IContentResult<BookPage>> GetAllAsync(CultureInfo culture)
    {
        try
        {
            var searchClient = SearchClient.Instance;
            var language = new Language(
                culture.Name,
                "default",
                culture.TwoLetterISOLanguageName,
                "porter",
                null,
                null,
                null
            );

            var results = searchClient.Search<BookPage>()
                .For(string.Empty)
                .Filter(x => x.Language.Name.Match(culture.Name))
                .GetContentResult();

            return Task.FromResult(results);
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR :  BooksPageService.SearchAllAsync() : {ex.Message}");
            return Task.FromResult<IContentResult<BookPage>>(null!);   
        }
    }
    public Task<IContentResult<BookPage>> GetAllByLanguageAsync(string lang)
    {
        try
        {
            var searchClient = SearchClient.Instance;
            var language = new Language(
                lang,
                "default",
                lang,
                "porter",
                null,
                null,
                null
            );

            var results = searchClient.Search<BookPage>()
                .For("story")
                .Filter(x => x.Language.Name.Match(lang))
                .GetContentResult();

            return Task.FromResult(results);
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR :  BooksPageService.SearchAllAsync() : {ex.Message}");
            return Task.FromResult<IContentResult<BookPage>>(null!);   
        }
    }

    public UnifiedSearchResults SearchBooks(string query, CultureInfo culture)
    {
        try
        {
            var searchClient = SearchClient.Instance;

            var language = new Language(
                culture.Name,
                "default",
                culture.TwoLetterISOLanguageName,
                "porter",
                null,
                null,
                null
            );

            return searchClient.UnifiedSearchFor(query, language).Track().GetResult();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR :  BooksPageService.SearchBooks() : {ex.Message}");
            return null!;
        }
    }

    public async Task<List<BookPageModel>> GetFilteredBookPages(string query, BooksPage currentPage, string category, List<CategoryModel> allUsedCategories)
    {
        try
        {
            var searchResult = await SearchAsync(query, currentPage.Language);
            var bookPages = searchResult.Items.Select(item => item).ToList();
            var bookPageModels = bookPages.Select(bookpage => BookPageFactory.CreateBookPageModel(bookpage, _urlResolver, allUsedCategories)).ToList();
            
            if (!string.IsNullOrEmpty(category))
            {
                bookPageModels = bookPageModels.Where(book => book.Categories.Any(cat => cat.Value == category)).ToList();
            }
            return bookPageModels;
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : BooksPageService.GetFilteredBookPages() : {ex.Message}");
        }
            return new List<BookPageModel>();   
    }
    public async Task<List<BookPageModel>> GetAllBookPages(BooksPage currentPage, List<CategoryModel> allCategories)
    {
        try
        {
            var searchResult = await GetAllAsync(currentPage.Language);
            var bookPages = searchResult.Items.Select(item => item).ToList();
            var bookPageModels = bookPages.Select(bookpage => BookPageFactory.CreateBookPageModel(bookpage, _urlResolver, allCategories)).ToList();
            
            return bookPageModels;
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : BooksPageService.GetAllFilteredBookPages() : {ex.Message}");
        }
            return new List<BookPageModel>();   
    }
    public async Task<List<BookPageModel>> GetAllByLanguageBookPages(string lang, List<CategoryModel> allCategories)
    {
        try
        {
            var searchResult = await GetAllByLanguageAsync(lang);
            var bookPages = searchResult.Items.Select(item => item).ToList();
            var bookPageModels = bookPages.Select(bookpage => BookPageFactory.CreateBookPageModel(bookpage, _urlResolver, allCategories)).ToList();
            
            return bookPageModels;
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : BooksPageService.GetAllFilteredBookPages() : {ex.Message}");
        }
            return new List<BookPageModel>();   
    }

    public async Task<BooksModel> GetFilteredBookPages(string query, BooksPage currentPage, string category, List<CategoryModel> allUsedCategories, int pageNumber, int pageSize)
    {
        try
        {
            var searchResult = await SearchAsync(query, currentPage.Language);
            var bookPages = searchResult.Items.Select(item => item).ToList();
            var bookPageModels = bookPages.Select(bookpage => BookPageFactory.CreateBookPageModel(bookpage, _urlResolver, allUsedCategories)).ToList();

            if (!string.IsNullOrEmpty(category))
            {
                bookPageModels = bookPageModels.Where(book => book.Categories.Any(cat => cat.Value == category)).ToList();
            }

            var paginatedBooks = bookPageModels
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new BooksModel { ResultCount = bookPageModels.Count, Books = paginatedBooks};
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : BooksPageService.GetFilteredBookPages() : {ex.Message}");
        }
        return new BooksModel();
    }
}
