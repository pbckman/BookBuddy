using BookBuddy.Models.Pages;
using BookBuddy.Models.QuizModels;
using BookBuddy.Models.ResultModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookBuddy.Business.Services.QuizService
{
    public interface IQuizService
    {
        
        

        QuizModel GetQuizData(int quizPageId, string lang);
        Task<QuizModel> CreateQuizAsync(QuizModel quiz, QuizResultModel result);
        QuizModel CreateQuiz(QuizModel quiz);
        bool IsAvailableChapter(ChapterModel chapter, QuizModel quiz);
        bool IsCompletedQuiz(QuizModel quiz);
        Task SaveToLocalStorageAsync(QuizModel quiz, string selectedOption);
        Task RemoveLocalStorageQuizState(int quizResultId);
        Task<List<QuizCardModel>> GetActiveQuizzesAsync(List<QuizResultModel> results, string lang);
        Task<int> GetQuizPageId(int bookId, string lang);
    }
}