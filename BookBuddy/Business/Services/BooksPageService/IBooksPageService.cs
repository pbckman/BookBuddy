using System;
using System.Globalization;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.Pages;
using EPiServer.Find.Cms;
using EPiServer.Find.UnifiedSearch;

namespace BookBuddy.Business.Services.BooksPageService;

public interface IBooksPageService
{
    UnifiedSearchResults SearchBooks(string query, CultureInfo culture);
    Task<IContentResult<BookPage>> SearchAsync(string query, CultureInfo culture);
    Task<IContentResult<BookPage>> GetAllAsync(CultureInfo culture);
    Task<IContentResult<BookPage>> GetAllByLanguageAsync(string lang);
    Task<List<BookPageModel>> GetFilteredBookPages(string query, BooksPage currentPage, string category, List<CategoryModel> allUsedCategories);
    Task<List<BookPageModel>> GetAllBookPages(BooksPage currentPage, List<CategoryModel> allUsedCategories);
    Task<List<BookPageModel>> GetAllByLanguageBookPages(string lang, List<CategoryModel> allCategories);
    Task<BooksModel> GetFilteredBookPages(string query, BooksPage currentPage, string category, List<CategoryModel> allUsedCategories, int pageNumber, int pageSize);
}
