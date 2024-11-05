using System;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.Pages;

namespace BookBuddy.Models.ViewModels;

public class BooksPageViewModel(BooksPage currentPage) : BasicPageViewModel<BooksPage>(currentPage)
{
    public string Query { get; set; } = string.Empty;
    // public List<TestData> Result { get; set; }
    // public UnifiedSearchResults Result { get; set; }
    public List<BookPageModel> Result { get; set; }
}
