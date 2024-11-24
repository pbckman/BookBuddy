using System;
using BookBuddy.Models.MyQuizzesModels;
using BookBuddy.Models.Pages;
using BookBuddy.Models.QuizModels;
using BookBuddy.Models.ResultModels;
using EPiServer.Web.Routing;

namespace BookBuddy.Business.Factories;

public class MyQuizzesFactory
{
    private readonly IContentLoader _contentLoader;
    private readonly IUrlResolver _urlResolver;
    // private readonly IQuizFactory _quizFactory;
    private readonly ILogger<MyQuizzesFactory> _logger;

    public MyQuizzesFactory(IContentLoader contentLoader, IUrlResolver urlResolver,  ILogger<MyQuizzesFactory> logger)
    {
        _contentLoader = contentLoader;
        _urlResolver = urlResolver;
        // _quizFactory = quizFactory;
        _logger = logger;
    }

    public MyQuizzesModel Create(QuizPage currentPage, QuizResultModel result)
    {
        try
        {
            var quiz = new MyQuizzesModel();
            var bookPage = _contentLoader.GetAncestors(currentPage.ContentLink).OfType<BookPage>().FirstOrDefault();
            var chapterPages = _contentLoader.GetChildren<ChapterPage>(currentPage.ContentLink, new LanguageSelector(currentPage.Language.Name));

            if (bookPage != null)
            {
                quiz.BookId = bookPage.ContentLink.ID;
                quiz.Title = bookPage.Title;
                quiz.ImageUrl = bookPage.ImageUrl;
                quiz.ImageAltText = bookPage.ImageAltText;
            }
            quiz.QuizId = currentPage.ContentLink.ID;
            quiz.PageUrl = UrlResolver.Current.GetUrl(currentPage.ContentLink);
            quiz.TotalCorrectAnswers = result.ChapterResults
                .SelectMany(c => c.QuestionResults)
                .Count(q => q.IsCorrect);
            quiz.TotalWrongAnswers = result.ChapterResults
                .SelectMany(c => c.QuestionResults)
                .Count(q => q.IsCorrect == false);
            quiz.IsCompleted = result.IsCompleted;

            
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

            quiz.TotalQuestions = quiz.Chapters.SelectMany(c => c.Questions).Count();
            quiz.TotalChapters = quiz.Chapters.Count;
            quiz.QuizResult = result;

            return quiz;
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : MyQuizzesFactory.Create : {ex.Message}");
        }

        return new MyQuizzesModel();
        
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
