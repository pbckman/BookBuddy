using BookBuddy.Models.Pages;
using BookBuddy.Models.Pages.Containers;

namespace BookBuddy.Business.Services.PageService
{
    public interface IPageService
    {
        AvailableBooksPage GetAvailableBooksPage(string name);
        List<BookPage> GetBookPages(string currentCulture);
        LanguageContainerPage GetLanguageContainerPage(string currentCulture);
        BooksPage GetBooksPage(string currentCulture);

        QuizPage GetQuizPageById(int quizPageId, string lang);
    }
}