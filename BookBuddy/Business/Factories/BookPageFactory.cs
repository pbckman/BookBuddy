using System;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.Pages;

namespace BookBuddy.Business.Factories;

public class BookPageFactory
{
    public static BookPageModel CreateBookPageModel(BookPage bookPage)
    {
        return new BookPageModel
        {
            Id = bookPage.ContentLink.ID,
            BookId = bookPage.BookId,
            Title = bookPage.Title,
            Authors = bookPage.Authors,
            Plot = bookPage.Plot,
            ImageUrl = bookPage.ImageUrl,
            ImageAltText = bookPage.ImageAltText
        };
    }
}
