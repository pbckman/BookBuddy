using System;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.Pages;

namespace BookBuddy.Models.ViewModels;

public class BooksPageViewModel(BooksPage currentPage, SiteSettingsPage siteSettings) : PageViewModel<BooksPage>(currentPage, siteSettings)
{
    public string Query { get; set; } = string.Empty;
    public List<BookPageModel> Result { get; set; } = new List<BookPageModel>();
    public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
}
