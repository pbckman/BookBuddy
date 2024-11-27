using BookBuddy.Models.QuizModels;
using BookBuddy.Models.ResultModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookBuddy.Business.Services.QuizResultService
{
    public interface IQuizResultService
    {
        Task<QuizResultModel> GetResultByQuizIdAsync(int profileId, int quizId);
        Task<QuizResultModel> CreateQuizResultAsync(int quizId, int profileId);
        Task<ChapterResultModel> SaveChapterResultAsync(ChapterModel chapter, int quizResultId);
        Task<QuizResultModel> CompleteQuizAsync(int quizResultId);
        Task<List<QuizResultModel>> GetResultsByProfileIdAsync(int profileId);
        Task<List<QuizResultModel>> GetResultsAsync(int profileId, string lang);
        Task<ResultStatus> GetResultStatusAsync(int profileId, int quizPageId);
    }
}