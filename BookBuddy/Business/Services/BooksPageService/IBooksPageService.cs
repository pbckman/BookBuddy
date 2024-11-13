using System;
using System.Globalization;
using BookBuddy.Models.Pages;
using EPiServer.Find.Cms;
using EPiServer.Find.UnifiedSearch;

namespace BookBuddy.Business.Services.BooksPageService;

public interface IBooksPageService
{
    public UnifiedSearchResults SearchBooks(string query, CultureInfo culture);
    public Task<IContentResult<BookPage>> SearchAsync(string query, CultureInfo culture);
}
