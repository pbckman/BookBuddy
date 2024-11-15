using System;
using System.Globalization;
using BookBuddy.Models.Pages;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Find.Framework.Statistics;
using EPiServer.Find.UnifiedSearch;

namespace BookBuddy.Business.Services.BooksPageService;

public class BooksPageService : IBooksPageService
{
    private readonly ILogger<BooksPageService> _logger;

    public BooksPageService(ILogger<BooksPageService> logger)
    {
        _logger = logger;
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

            var results = searchClient.Search<BookPage>()
                .For(query)
                .Filter(x => x.Language.Name.Match(culture.Name))
                .GetContentResult();

            return Task.FromResult(results);
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR :  BooksPageService.SearchAsync() : {ex.Message}");
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
}
