using BookBuddy.Business.Factories;
using BookBuddy.Business.Services.PageService;
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
        private readonly IPageService _pageService;

        public QuizResultService(DataContext dbContext, QuizResultFactory resultFactory, IPageService pageService)
        {
            _dbContext = dbContext;
            _resultFactory = resultFactory;
            _pageService = pageService;
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

        public async Task<List<QuizResultModel>> GetResultsAsync(int profileId, string lang)
        {
            try
            {
                var quizPages = _pageService.GetQuizPages(lang);
                if (quizPages == null)
                    return null!;

                var quizIds = quizPages.Select(quiz => quiz.ContentLink.ID).ToList();

                var results = (await _dbContext.QuizResults.Include(x => x.Profile)
                                                    .Include(x => x.ChapterResults)
                                                    .ThenInclude(x => x.QuestionResults)
                                                    .Where(x => x.ProfileId == profileId && quizIds.Contains(x.QuizId))
                                                    .ToListAsync()).Select(_resultFactory.Create).ToList();

                if (results != null)
                    return results;
            }
            catch (Exception)
            {
            }


            return null!;
        }

        public async Task<ResultStatus> GetResultStatusAsync(int profileId, int quizPageId)
        {
            var result = await _dbContext.QuizResults.FirstOrDefaultAsync(x => x.ProfileId == profileId && x.QuizId == quizPageId);

            if (result == null)
                return ResultStatus.None;

            if(result.IsCompleted)
                return ResultStatus.Completed;
           
            return ResultStatus.InProgress;
        }
    }

}
