using BookBuddy.Business.Extensions;
using BookBuddy.Business.Services.AiService;
using BookBuddy.Business.Services.BookContentService;
using BookBuddy.Business.Services.PageService;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.DDS;
using BookBuddy.Models.Pages;
using BookBuddy.Models.Pages.Containers;
using EPiServer.Data.Dynamic;

namespace BookBuddy.Business.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly IAiService _aiService;
        private readonly IBookContentService _bookContentService;
        private readonly ILogger<IBookService> _logger;
        private readonly IPageService _pageService;
        private readonly IContentLoader _contentLoader;

        public BookService(IAiService aiService, IBookContentService bookContentService, ILogger<IBookService> logger, IPageService pageService, IContentLoader contentLoader)
        {
            _aiService = aiService;
            _bookContentService = bookContentService;
            _logger = logger;
            _pageService = pageService;
            _contentLoader = contentLoader;
        }

        public List<BookModel> GetAllBooks(string currentCulture)
        {
            var store = DynamicDataStoreFactory.Instance.GetStore(typeof(Book));
            var allBooks = store.Items<Book>().Where(book => book.Language == currentCulture).ToList();
            if (allBooks.Count > 0)
            {
                return allBooks.Select(book => new BookModel
                {
                    BookId = book.BookId,
                    Title = book.Title
                }).ToList();
            }

            return null!;
        }

        public async Task<QuizBookModel> GetBookQuizAsync(int bookId, string lang)
        {
            var bookQuiz = new QuizBookModel();

            lang = lang == "sv" ? "swedish" : "english";

            try
            {
                var bookContent = await _bookContentService.GetBookContentAsync(bookId);

                bookQuiz.Metadata = await _bookContentService.GetBookMetadataAsync(bookId);

                MetadataDetails details = await _aiService.GenerateMetadataAsync(bookContent, lang);

                if (details != null)
                {
                    bookQuiz.Metadata.Title = details.Title;
                    bookQuiz.Metadata.Plot = details.Plot;
                }

                var chapterContentList = await _bookContentService.GetBookChaptersContentAsync(bookId);

                if (chapterContentList == null)
                    return null!;

                foreach (string chapterContent in chapterContentList)
                {
                    QuizChapterModel chapterQuiz = await _aiService.GenerateQuizQuestionsAsync(chapterContent, bookQuiz.Metadata.Title, lang);
                    if (chapterQuiz != null)
                    {
                        chapterQuiz.ChapterTitle = chapterQuiz.ChapterTitle.ToTitleCase();
                        bookQuiz.Chapters.Add(chapterQuiz);
                    }

                }

                return bookQuiz;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while generating quiz questions for book with id {bookId}");
            }

            return null!;
        }

        public List<int> GetCurrentBooks(string currentCulture)
        {
            LanguageContainerPage languageContainerPage = _pageService.GetLanguageContainerPage(currentCulture);
            if (languageContainerPage == null)
            {
                return null!;
            }

            List<BookPage> bookPages = _pageService.GetBookPages(currentCulture);
            if (bookPages.Count > 0)
            {
                return bookPages.Select(bookPage => bookPage.BookId).ToList();
            }


            return [];
        }
    }
}
