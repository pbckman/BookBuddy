using BookBuddy.Models.ResultModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookBuddy.Business.Services.QuizService
{
    public interface IQuizService
    {
        Task<QuizResultModel> GetResultByQuizIdAsync(int profileId, int quizId);
    }
}