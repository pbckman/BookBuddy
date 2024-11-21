using BookBuddy.Models.QuizModels;
using BookBuddy.Models.ResultModels;

namespace BookBuddy.Business.Services.QuizResultService
{
    public interface IQuizResultService
    {
        Task<QuizResultModel> GetResultByQuizIdAsync(int profileId, int quizId);
        Task<QuizResultModel> CreateQuizResultAsync(int quizId, int profileId);
        Task<ChapterResultModel> SaveChapterResultAsync(ChapterModel chapter, int quizResultId);
        Task<QuizResultModel> CompleteQuizAsync(int quizResultId);
    }
}