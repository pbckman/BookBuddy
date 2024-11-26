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
                quiz.NextQuestionBtnText = currentPage.Language.Name == "sv" ? "Nästa" : "Next";
                quiz.SubmitBtnText = currentPage.Language.Name == "sv" ? "Avsluta kaptitlet" : "End chapter";
                quiz.StartBtnText = currentPage.Language.Name == "sv" ? "Starta" : "Begin";
                quiz.NextChapterBtnText = currentPage.Language.Name == "sv" ? "Nästa kapitel" : "Next chapter";
                quiz.QuizResultTitle = currentPage.Language.Name == "sv" ? "Resultat" : "Result";



                foreach (var chapterPage in chapterPages)
                {
                    var questionPages = _contentLoader.GetChildren<QuestionPage>(chapterPage.ContentLink, new LanguageSelector(currentPage.Language.Name));
                    quiz.Chapters.Add(new ChapterModel
                    {
                        ChapterId = chapterPage.ContentLink.ID,
                        ChapterTitle = chapterPage.Name,
                        Questions = GetQuestions(questionPages.ToList()),
                       
                        ChapterSelectionSummary = new ChapterSelectionSummaryModel
                        {
                            ChapterId = chapterPage.ContentLink.ID,
                            Title = currentPage.Language.Name == "en" ? "Summary" : "Sammanfattning"
                        }

                    });;
                }

                return quiz;
            }
            catch (Exception ex)
            {
            }

            return new QuizModel();
           
        }



        public List<QuestionModel> GetQuestions(List<QuestionPage> questionPages)
        {
            var result = new List<QuestionModel>();
            for(int i = 0; i < questionPages.Count(); i++)
            {
                result.Add(new QuestionModel
                {
                    QuestionId = questionPages[i].ContentLink.ID,
                    QuestionNumber = i + 1,
                    QuestionTitle = questionPages[i].Name,
                    Question = questionPages[i].Question,
                    CorrectAnswer = questionPages[i].CorrectAnswer,
                    Options = new List<OptionModel>
                    {
                        new()
                        {
                            OptionValue = questionPages[i].AnswerAValue,
                            OptionText = questionPages[i].AnswerAText,
                        },
                        new()
                        {
                            OptionValue = questionPages[i].AnswerBValue,
                            OptionText = questionPages[i].AnswerBText,
                        },
                        new()
                        {
                            OptionValue = questionPages[i].AnswerCValue,
                            OptionText = questionPages[i].AnswerCText,
                        },
                        new()
                        {
                            OptionValue = questionPages[i].AnswerDValue,
                            OptionText = questionPages[i].AnswerDText,
                        }
                    }
                });
            }

            return result;
        }

   
    }
}
