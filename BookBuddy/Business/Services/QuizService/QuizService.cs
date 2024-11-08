using BookBuddy.Business.Factories;
using BookBuddy.Data.Contexts;
using BookBuddy.Models.ResultModels;
using Microsoft.EntityFrameworkCore;

namespace BookBuddy.Business.Services.QuizService
{
    public class QuizService : IQuizService
    {
        private readonly DataContext _dbContext;
        private readonly IQuizFactory _quizFactory;

        public QuizService(DataContext dbContext, IQuizFactory quizFactory)
        {
            _dbContext = dbContext;
            _quizFactory = quizFactory;
        }

        public async Task<QuizResultModel> GetResultByQuizIdAsync(int profileId, int quizId)
        {
            try
            {
                var result = await _dbContext.QuizResults
                                        .Include(x => x.Profile)
                                        .Include(x => x.ChapterResults)
                                        .ThenInclude(x => x.QuestionResults)
                                        .FirstOrDefaultAsync(quiz => quiz.Profile.Id == profileId && quiz.QuizId == quizId);
                
                if(result != null)
                    return _quizFactory.Create(result);

            }
            catch (Exception ex)
            {
            }

            return null!;
        }
    }
}
