using BookBuddy.Business.Factories;
using BookBuddy.Business.Services.BooksPageService;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.Pages;
using EPiServer.Web.Routing;

namespace BookBuddy.Business.Services.StartPageService
{
    public class StartPageService
    {
        private readonly IBooksPageService _booksPageService;
        private readonly UrlResolver _urlResolver;

        public StartPageService(IBooksPageService booksPageService, UrlResolver urlResolver)
        {
            _booksPageService = booksPageService;
            _urlResolver = urlResolver;
        }

        public async Task<List<BookPageModel>> GetBooksAsync(StartPage currentPage)
        {
            
            var searchResult = _booksPageService.Search(null!,currentPage.Language);
            var bookPages = searchResult.Items.Select(item => item).ToList();
            var bookPageModels = bookPages.Select(bookpage => BookPageFactory.CreateBookPageModel(bookpage, _urlResolver)).ToList();


            return bookPageModels;

        }
    }
}
