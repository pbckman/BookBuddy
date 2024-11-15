using Blazored.LocalStorage;
using BookBuddy.Business.Factories;
using BookBuddy.Business.Services.PageService;
using BookBuddy.Data.Contexts;
using BookBuddy.Data.Entities;
using BookBuddy.Models.Pages;
using BookBuddy.Models.QuizModels;
using BookBuddy.Models.ResultModels;
using BootstrapBlazor.Components;
using EPiServer.Find;
using Microsoft.EntityFrameworkCore;

namespace BookBuddy.Business.Services.QuizService
{
    public class QuizService : IQuizService
    {
        private readonly DataContext _dbContext;
        private readonly IQuizFactory _quizFactory;
        private readonly QuizResultFactory _resultFactory;
        private readonly IPageService _pageService;
        private readonly ILocalStorageService _localStorageService;

        public QuizService(DataContext dbContext, IQuizFactory quizFactory, QuizResultFactory resultFactory, IPageService pageService, ILocalStorageService localStorageService)
        {
            _dbContext = dbContext;
            _quizFactory = quizFactory;
            _resultFactory = resultFactory;
            _pageService = pageService;
            _localStorageService = localStorageService;
        }

        public async Task<QuizModel> CreateQuizAsync(QuizModel quiz, QuizResultModel result)
        {
            
            quiz.QuizResult = result;

            if (IsCompletedQuiz(quiz))
            {
                quiz.IsCompleted = true;
                quiz.Display = Display.QuizSummary;
            }
            else
            {
                if (result.ChapterResults.Count > 0)
                {
                    quiz.CurrentChapter = quiz.Chapters[result.ChapterResults.Count()];
                    quiz.NextAvailableChapter = quiz.CurrentChapter;
                    quiz.CurrentQuestion = null;
                }
                else
                {
                    quiz.CurrentChapter = quiz.Chapters.FirstOrDefault();
                    quiz.CurrentChapter!.ShowIntro = true;
                    quiz.CurrentChapter!.ShowQuestions = true;
                    quiz.CurrentChapter!.IsStarted = true;
                    quiz.CurrentQuestion = null;
                }

                var savedQuizState = await _localStorageService.GetItemAsync<LSQuizState>($"QuizState - {quiz.QuizResult.Id}");
                if (savedQuizState != null)
                {
                    var chapter = quiz.Chapters.FirstOrDefault(x => x.ChapterId == savedQuizState.ChapterId);
                    if(chapter != null)
                    {
                        quiz.CurrentChapter = chapter;
                        quiz.CurrentChapter.ShowQuestions = true;
                        quiz.CurrentChapter.IsStarted = true;
                        quiz.CurrentQuestion = null;

                        quiz.Display = Display.ChapterIntro;

                        foreach (var question in chapter.Questions)
                        {
                            if (savedQuizState.AnsweredQuestions.Any(x => x.QuestionId == question.QuestionId))
                            {
                                question.IsAnswerd = true;
                                question.Options.FirstOrDefault(option => option.OptionValue == savedQuizState.AnsweredQuestions.FirstOrDefault(x => x.QuestionId == question.QuestionId)!.SelectedOption)!.IsSelected = true;

                            }
                        }
                    }
                    
                }
                else if (quiz.CurrentChapter != null)
                {
                    quiz.CurrentChapter.ShowIntro = true;
                    quiz.CurrentQuestion = null;
                    quiz.Display = Display.ChapterIntro;
                }

            }
          

            return quiz;
        }

        public QuizModel CreateQuiz(QuizModel quiz)
        {
            quiz.QuizResult = null;
            quiz.CurrentChapter = quiz.Chapters.FirstOrDefault();
            quiz.CurrentQuestion = null;

            if (quiz.CurrentChapter != null)
            {
                quiz.CurrentChapter.ShowIntro = true;
                quiz.CurrentQuestion = null;
                quiz.Display = Display.ChapterIntro;
            }

            return quiz;
        }

        public QuizModel? GetQuizData(int quizPageId, string lang)
        {
            var currentQuizPage = _pageService.GetQuizPageById(quizPageId, lang);
            if (currentQuizPage == null)
                return null;

            var quiz = _quizFactory.Create(currentQuizPage);

            return quiz ?? null;
        }

        public bool IsAvailableChapter(ChapterModel chapter, QuizModel quiz)
        {
            return  chapter == quiz.CurrentChapter ||
                    chapter == quiz.NextAvailableChapter ||
                    quiz.IsCompleted ||
                    (quiz.QuizResult != null && quiz.QuizResult.ChapterResults.Any(x => x.ChapterId == chapter.ChapterId));
        }

        public bool IsCompletedQuiz(QuizModel quiz)
        {
            return quiz.QuizResult != null && quiz.QuizResult.ChapterResults.Count == quiz.Chapters.Count;
        }

        public async Task SaveToLocalStorageAsync(QuizModel quiz, string selectedOption)
        {
            var savedQuizState = await _localStorageService.GetItemAsync<LSQuizState>($"QuizState - {quiz.QuizResult!.Id}");
            if (savedQuizState == null)
            {
                var quizState = new LSQuizState
                {
                    ChapterId = quiz.CurrentChapter!.ChapterId,
                    AnsweredQuestions = new List<LSQuestionModel>
                {

                    new LSQuestionModel
                    {
                        QuestionId = quiz.CurrentQuestion!.QuestionId,
                        SelectedOption = selectedOption
                    }
                }

                };

                await _localStorageService.SetItemAsync($"QuizState - {quiz.QuizResult!.Id}", quizState);
            }
            else if (!savedQuizState.AnsweredQuestions.Any(x => x.QuestionId == quiz.CurrentQuestion!.QuestionId))
            {
                savedQuizState.AnsweredQuestions.Add(new LSQuestionModel
                {
                    QuestionId = quiz.CurrentQuestion!.QuestionId,
                    SelectedOption = selectedOption
                });

                await _localStorageService.SetItemAsync($"QuizState - {quiz.QuizResult!.Id}", savedQuizState);
            }
            else
            {
                var question = savedQuizState.AnsweredQuestions.FirstOrDefault(x => x.QuestionId == quiz.CurrentQuestion!.QuestionId);
                if (question != null)
                {
                    question.SelectedOption = selectedOption;
                    await _localStorageService.SetItemAsync($"QuizState - {quiz.QuizResult!.Id}", savedQuizState);
                }
            }

          
        }

        public async Task RemoveLocalStorageQuizState(int quizResultId)
        {
            var savedProfileState = await _localStorageService.GetItemAsync<LSQuizState>($"QuizState - {quizResultId}");
            if (savedProfileState != null)
            {
                await _localStorageService.RemoveItemAsync($"QuizState - {quizResultId}");
            }
        }
    }
}
