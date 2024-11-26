using BookBuddy.Models.DataModels;
using BookBuddy.Models.Pages;

namespace BookBuddy.Business.Services.StartPageService
{
    public class StartPageService
    {
        private readonly IBooksPageService _booksPageService;
        private readonly UrlResolver _urlResolver;
        private readonly ILogger<StartPageService> _logger;

        public StartPageService(IBooksPageService booksPageService, UrlResolver urlResolver, ILogger<StartPageService> logger)
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

        }
    }
}
