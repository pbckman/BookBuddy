using BookBuddy.Data.Entities;
using BookBuddy.Models.QuizModels;
using BookBuddy.Models.ResultModels;

namespace BookBuddy.Business.Factories
{
    public class QuizResultFactory
    {
        public ChapterResultModel CreateChapterResult(ChapterResultEntity entity)
        {
         
            return new ChapterResultModel
            {
                QuizId = entity.QuizResult.QuizId,
                ChapterId = entity.ChapterId,
                QuestionResults = entity.QuestionResults.Select(q => new QuestionResultModel
                {
                    QuestionId = q.QuestionId,
                    SelectedOption = q.SelectedOption,
                    CorrectAnswer = q.CorrectAnswer,
                    IsCorrect = q.IsCorrect
                }).ToList()
            };

        }

        public QuizResultModel Create(QuizResultEntity result)
        {
            var resultModel = new QuizResultModel
            {
                Id = result.Id,
                ProfileId = result.ProfileId,
                QuizId = result.QuizId,
                IsCompleted = result.IsCompleted,
                ChapterResults = result.ChapterResults.Select(x => new ChapterResultModel
                {
                    ChapterId = x.ChapterId,
                    QuestionResults = x.QuestionResults.Select(q => new QuestionResultModel
                    {
                        QuestionId = q.QuestionId,
                        SelectedOption = q.SelectedOption,
                        IsCorrect = q.IsCorrect,
                        CorrectAnswer = q.CorrectAnswer
                    }).ToList()
                }).ToList(),

            };

            return resultModel;
        }


    }
}
