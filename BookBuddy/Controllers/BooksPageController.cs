using System;
using BookBuddy.Business.Services.BooksPageService;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers;

public class BooksPageController(IContentLoader contentLoader, IBooksPageService booksPageService) : PageControllerBase<BooksPage>
{
    public readonly IContentLoader _contentLoader = contentLoader;
    public readonly IBooksPageService _booksPageService = booksPageService;

    public IActionResult Index(BooksPage currentPage)
    {
        var model = new BooksPageViewModel(currentPage);
        return View("Index", model);
    }

    // public IActionResult Search(BooksPage currentPage, string query)
    // {
    //     var searchResult = _booksPageService.Search(query, currentPage.Language);
    //     var bookPages = searchResult.Items.Select(item => item).ToList();
    //     var bookPageModels = bookPages.Select(PageDataFactory.CreateBookPage).ToList();
    //     var model = new BooksPageViewModel(currentPage)
    //     {
    //         Query = query,
    //         Result = _booksPageService.SearchBooks(query, currentPage.Language)
    //     };
    //     return View("Index", model);
    // }
}
