using BookBuddy.Models.DataModels;

namespace BookBuddy.Business.Services.BookPageService
{
    public interface IBookPageService
    {
        bool CreatePages(QuizBookModel bookQuizModel, string selectedLanguage);
    }
}