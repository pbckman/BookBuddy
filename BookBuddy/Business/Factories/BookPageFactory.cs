using System;
using System.Diagnostics;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using EPiServer.Web.Routing;

namespace BookBuddy.Business.Factories;

public class BookPageFactory
{
    public static BookPageModel CreateBookPageModel(BookPage bookPage, UrlResolver urlResolver,List<CategoryModel> allCategories)
    {
        try
        {
            var contentLink = bookPage.ContentLink;
            var pageUrl = urlResolver.GetUrl(contentLink);

            return new BookPageModel
            {
                Id = bookPage.ContentLink.ID,
                BookId = bookPage.BookId,
                Title = bookPage.Title,
                Authors = bookPage.Authors,
                Plot = bookPage.Plot,
                ImageUrl = bookPage.ImageUrl,
                ImageAltText = bookPage.ImageAltText,
                PageUrl = pageUrl,
                Categories = bookPage.Categories?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(category => allCategories.FirstOrDefault(c => c.Value == category) ?? new CategoryModel { Text = category, Value = category })
                    .ToList() ?? new List<CategoryModel>()
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ERROR : BookPageFactory.CreateBookPageModel() : {ex.Message}");
            return new BookPageModel();
        }
    }

    public static BooksPageViewModel CreateBooksPageViewModel(BooksPage currentPage, SiteSettingsPage siteSettings, string query, List<BookPageModel> bookPageModels, List<CategoryModel> allUsedCategories)
    {
        return new BooksPageViewModel(currentPage, siteSettings)
        {
            Query = query,
            Result = bookPageModels,
            Categories = allUsedCategories
        };
    }
}
