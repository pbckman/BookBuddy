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
    public Task<IContentResult<BookPage>> SearchAsync(string query, CultureInfo culture)
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

    public UnifiedSearchResults SearchBooks(string query, CultureInfo culture)
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
}
