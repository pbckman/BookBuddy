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
        private readonly ILogger<StartPageService> _logger;

        public StartPageService(IBooksPageService booksPageService, UrlResolver urlResolver,ILogger<StartPageService> logger)
        {
            _booksPageService = booksPageService;
            _urlResolver = urlResolver;
            _logger = logger;
        }

        public async Task<List<BookPageModel>> GetBooksAsync(StartPage currentPage)
        {
            var searchResult = await _booksPageService.SearchAsync(null!, currentPage.Language);

            if (searchResult == null || searchResult.Items == null)
            {
                _logger.LogError("No books found for the specified language");
                return new List<BookPageModel>();
            }

            var bookPages = searchResult.Items.ToList();
            var bookPageModel = bookPages
                .Select(bookspage => BookPageFactory.CreateBookPageModel(bookspage, _urlResolver, null))
                .ToList();

            return bookPageModel;
            //var searchResult = _booksPageService.SearchAsync(null!,currentPage.Language);
            //var bookPages = searchResult.Items.Select(item => item).ToList();
            //var bookPageModels = bookPages.Select(bookpage => BookPageFactory.CreateBookPageModel(bookpage, _urlResolver, null)).ToList();


            //return bookPageModels;

        }
    }
}
