using BookBuddy.Data.Entities;
using BookBuddy.Models.Pages;
using BookBuddy.Models.QuizModels;
using BookBuddy.Models.ResultModels;

namespace BookBuddy.Business.Factories
{
    public class QuizFactory : IQuizFactory
    {
        private readonly IContentLoader _contentLoader;

        public QuizFactory(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public QuizModel Create(QuizPage currentPage)
        {
            try
            {
                var quiz = new QuizModel();
                var bookPage = _contentLoader.GetAncestors(currentPage.ContentLink).OfType<BookPage>().FirstOrDefault();
                var chapterPages = _contentLoader.GetChildren<ChapterPage>(currentPage.ContentLink, new LanguageSelector(currentPage.Language.Name));

                quiz.Id = currentPage.ContentLink.ID;
                quiz.Title = bookPage.Title;
                quiz.ImageUrl = bookPage.ImageUrl;
                quiz.ImageAltText = bookPage.ImageAltText;

                foreach (var chapterPage in chapterPages)
                {
                    var questionPages = _contentLoader.GetChildren<QuestionPage>(chapterPage.ContentLink, new LanguageSelector(currentPage.Language.Name));
                    quiz.Chapters.Add(new ChapterModel
                    {
                        ChapterId = chapterPage.ContentLink.ID,
                        ChapterTitle = chapterPage.Name,
                        Questions = questionPages.Select(q => new QuestionModel
                        {
                            QuestionId = q.ContentLink.ID,
                            QuestionTitle = q.Name,
                            Question = q.Question,
                            CorrectAnswer = q.CorrectAnswer,
                            Options = new Dictionary<string, string>
                        {
                            { q.AnswerAValue, q.AnswerAText },
                            { q.AnswerBValue, q.AnswerBText },
                            { q.AnswerCValue, q.AnswerCText },
                            { q.AnswerDValue, q.AnswerDText },
                        }
                        }).ToList()

                    });
                }

                return quiz;
            }
            catch (Exception ex)
            {
            }

            return new QuizModel();
           
        }

        public QuizResultModel Create(QuizResultEntity result)
        {
            return new QuizResultModel
            {
                Id = result.Id,
                QuizId = result.QuizId,
                ChapterResults = result.ChapterResults.Select(x => new ChapterResultModel
                {
                    Id = x.Id,
                    ChapterId = x.ChapterId,
                    QuestionResults = x.QuestionResults.Select(q => new QuestionResultModel
                    {
                        Id = q.Id,
                        QuestionId = q.QuestionId,
                        SelectedOption = q.SelectedOption,
                        IsCorrect = q.IsCorrect,
                        CorrectAnswer = q.CorrectAnswer
                    }).ToList()
                }).ToList(),

            };
        }
    }
}
