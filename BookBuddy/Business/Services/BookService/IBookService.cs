using BookBuddy.Models.DataModels;

namespace BookBuddy.Business.Services.BookService
{
    public interface IBookService
    {
        List<BookModel> GetAllBooks(string currentCulture);
        Task<QuizBookModel> GetBookQuizAsync(int bookId, string lang);
        List<int> GetCurrentBooks(string currentCulture);
    }
}