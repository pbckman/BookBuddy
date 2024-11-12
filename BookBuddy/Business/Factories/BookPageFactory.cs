using System;
using System.Diagnostics;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.Pages;
using EPiServer.Web.Routing;

namespace BookBuddy.Business.Factories;

public class BookPageFactory
{
    public static BookPageModel CreateBookPageModel(BookPage bookPage, UrlResolver urlResolver)
    {
        var contentLink = bookPage.ContentLink;
        var pageUrl = urlResolver.GetUrl(contentLink);

        // Debugging output
        Console.WriteLine($"ContentLink: {contentLink}");
        Console.WriteLine($"PageUrl: {pageUrl}");
        return new BookPageModel
        {
            Id = bookPage.ContentLink.ID,
            BookId = bookPage.BookId,
            Title = bookPage.Title,
            Authors = bookPage.Authors,
            Plot = bookPage.Plot,
            ImageUrl = bookPage.ImageUrl,
            ImageAltText = bookPage.ImageAltText,
            PageUrl = pageUrl
        };
    }
}
