using BookBuddy.Business.Factories;
using BookBuddy.Data.Contexts;
using BookBuddy.Data.Entities;
using BookBuddy.Models.QuizModels;
using BookBuddy.Models.ResultModels;
using Microsoft.EntityFrameworkCore;

namespace BookBuddy.Business.Services.QuizResultService
{
    public class QuizResultService : IQuizResultService
    {
        private readonly DataContext _dbContext;
        private readonly QuizResultFactory _resultFactory;

        public QuizResultService(DataContext dbContext, QuizResultFactory resultFactory)
        {
            _dbContext = dbContext;
            _resultFactory = resultFactory;
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

                if (result != null)
                    return _resultFactory.Create(result);

            }
            catch (Exception ex)
            {
            }

            return null!;
        }

        public async Task<QuizResultModel> CreateQuizResultAsync(int quizId, int profileId)
        {
            var quizResult = new QuizResultEntity
            {
                QuizId = quizId,
                ProfileId = profileId
            };

            _dbContext.QuizResults.Add(quizResult);
            await _dbContext.SaveChangesAsync();

            return _resultFactory.Create(quizResult);
        }

        public async Task<ChapterResultModel> SaveChapterResultAsync(ChapterModel chapter, int quizResultId)
        {
            if (await _dbContext.ChapterResults.AnyAsync(x => x.ChapterId == chapter.ChapterId && x.QuizResultId == quizResultId))
                return null!;

            var chapterResult = new ChapterResultEntity
            {
                QuizResultId = quizResultId,
                ChapterId = chapter.ChapterId,

                QuestionResults = chapter.Questions.Select(q => new QuestionResultEntity
                {
                    QuestionId = q.QuestionId,
                    SelectedOption = q.Options.FirstOrDefault(o => o.IsSelected)!.OptionValue,
                    IsCorrect = q.Options.FirstOrDefault(o => o.IsSelected)!.OptionValue == q.CorrectAnswer,
                    CorrectAnswer = q.CorrectAnswer,
                    ChapterResultId = chapter.ChapterId
                }).ToList()
            };
            if (await _dbContext.QuizResults.AnyAsync(x => x.Id == quizResultId))
            {
                _dbContext.ChapterResults.Add(chapterResult);
                await _dbContext.SaveChangesAsync();
            }


            return _resultFactory.CreateChapterResult(chapterResult);
        }

        public async Task<QuizResultModel> CompleteQuizAsync(int quizResultId)
        {
            try
            {
                var quizResult = await _dbContext.QuizResults.FirstOrDefaultAsync(x => x.Id == quizResultId);
                if (quizResult != null)
                {
                    quizResult.IsCompleted = true;
                    _dbContext.QuizResults.Update(quizResult);
                    await _dbContext.SaveChangesAsync();

                    return _resultFactory.Create(quizResult);
                }
            }
            catch (Exception)
            {

            }

            return null!;
           
        }

      

        public async Task<List<QuizResultModel>> GetResultsByProfileIdAsync(int profileId)
        {
            try
            {
                var results = await _dbContext.QuizResults.Include(QuizResult => QuizResult.ChapterResults)
                                                          .ThenInclude(chapterResult => chapterResult.QuestionResults)
                                                          .Where(QuizResult => QuizResult.ProfileId == profileId)
                                                          .Select(QuizResult => _resultFactory.Create(QuizResult))
                                                          .ToListAsync();
                if (results != null)
                    return results;
            }
            catch (Exception ex) {}

            return null!;
        }
    }

}
