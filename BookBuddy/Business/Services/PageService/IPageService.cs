using BookBuddy.Models.Pages;
using BookBuddy.Models.Pages.Containers;
using EPiServer.Find;

namespace BookBuddy.Business.Services.PageService
{
    public interface IPageService
    {
        AvailableBooksPage GetAvailableBooksPage(string name);
        List<BookPage> GetBookPages(string currentCulture);
        LanguageContainerPage GetLanguageContainerPage(string currentCulture);
        BooksPage GetBooksPage(string currentCulture);

        QuizPage GetQuizPageById(int quizPageId, string lang);
        MyQuizzesPage GetMyQuizzesPage(string lang);
        AchievementsPage GetAchievementsPage(string lang);
        List<QuizPage> GetQuizPages(string lang);
    }
}