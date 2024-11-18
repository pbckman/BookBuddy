using BookBuddy.Business.Services.QuizService;
using BookBuddy.Models.QuizModels;

namespace BookBuddy.Business.Services.StateService
{
    public class StateService : IStateService
    {
        private readonly IQuizService _quizService;

        public StateService(IQuizService quizService)
        {
            _quizService = quizService;
        }
        public QuizModel UpdateSelectedQuestionState(ChapterModel chapter, QuestionModel question, QuizModel quiz)
        {
            quiz.CurrentChapter = chapter;
            quiz.CurrentQuestion = question;
            quiz.CurrentChapter.ShowIntro = false;
            quiz.Display = Display.Question;

            return quiz;
        }

        public QuizModel UpdateSelectedChapterState(ChapterModel chapter, QuizModel quiz)
        {
            if (quiz.CurrentChapter != null)
            {
                chapter.ShowIntro = true;
                if (chapter != quiz.CurrentChapter)
                    quiz.CurrentChapter.ShowIntro = false;

                quiz.CurrentChapter = chapter;
                if (quiz.QuizResult != null && quiz.QuizResult.ChapterResults.Any(x => x.ChapterId == quiz.CurrentChapter.ChapterId))
                    quiz.CurrentChapter.ShowQuestions = !quiz.CurrentChapter.ShowQuestions;

                quiz.CurrentQuestion = null;
                quiz.Display = Display.ChapterIntro;
            }
            else
            {
                quiz.CurrentChapter = chapter;
                chapter.ShowIntro = true;
                quiz.CurrentChapter.ShowQuestions = true;
                quiz.Display = Display.ChapterIntro;
            }

            return quiz;
        }


        public QuizModel UpdateCurrentStateByQuestionId(int questionId, QuizModel quiz)
        {
            quiz.CurrentChapter = quiz.Chapters.FirstOrDefault(c => c.Questions.Any(q => q.QuestionId == questionId));

            if (quiz.CurrentChapter != null)
            {
                quiz.CurrentQuestion = quiz.CurrentChapter.Questions.FirstOrDefault(q => q.QuestionId == questionId);
            }

            return quiz;
        }

        public bool IsFinishedChapter(ChapterModel chapter, QuizModel quiz)
        {
            return chapter.IsFinished || (quiz.QuizResult != null && quiz.QuizResult.ChapterResults.Any(x => x.ChapterId == chapter.ChapterId));
        }

        public QuizModel UpdateSelectedSummaryState(ChapterModel chapter, QuizModel quiz)
        {
            quiz.CurrentChapter = chapter;
            quiz.CurrentQuestion = null;
            quiz.CurrentChapter.IsFinished = true;
            quiz.CurrentChapter.IsStarted = false;
            quiz.CurrentChapter.ShowIntro = false;
            quiz.Display = Display.ChapterSummary;

            return quiz;
        }

        public string GetQuestionState(QuestionModel question, QuizModel quiz)
        {
            if (quiz.QuizResult != null)
            {
                var chapter = quiz.Chapters.FirstOrDefault(x => x.Questions.Any(x => x.QuestionId == question.QuestionId));
                if (chapter != null)
                {
                    var chapterResult = quiz.QuizResult.ChapterResults.FirstOrDefault(x => x.ChapterId == chapter.ChapterId);
                    if (chapterResult != null)
                    {
                        var questionResult = chapterResult.QuestionResults.FirstOrDefault(x => x.QuestionId == question.QuestionId);
                        if (questionResult != null)
                        {
                            return questionResult.IsCorrect ? "correct" : "incorrect";
                        }
                    }
                    else if (question.Options.Any(x => x.IsSelected))
                        return "answered";
                }

            }
            else if (question.Options.Any(x => x.IsSelected))
                return "answered";


            return "";
        }

        public bool IsAvailableChapter(ChapterModel chapter, QuizModel quiz)
        {
            return chapter == quiz.CurrentChapter || chapter.IsFinished || chapter.IsStarted || chapter == quiz.NextAvailableChapter || quiz.IsCompleted || (quiz.QuizResult != null && quiz.QuizResult.ChapterResults.Any(x => x.ChapterId == chapter.ChapterId));
        }

        public QuizModel SetQuizStartState(QuizModel quiz)
        {
            quiz.CurrentChapter!.ShowIntro = false;
            quiz.CurrentQuestion = quiz.CurrentChapter.Questions.FirstOrDefault();
            quiz.CurrentChapter.ShowQuestions = true;
            quiz.Display = Display.Question;
            quiz.CurrentChapter.IsStarted = true;
            quiz.NextAvailableChapter = null;

            quiz.Chapters.Where(c => c != quiz.CurrentChapter).ToList().ForEach(c => c.ShowQuestions = false);

            return quiz;
        }

        public string GetOptionState(OptionModel option, QuizModel quiz)
        {
            if (quiz.QuizResult != null)
            {
                var chapterResult = quiz.QuizResult.ChapterResults.FirstOrDefault(x => x.ChapterId == quiz.CurrentChapter!.ChapterId);
                if (chapterResult != null)
                {
                    var questionResult = chapterResult.QuestionResults.FirstOrDefault(x => x.QuestionId == quiz.CurrentQuestion!.QuestionId);
                    if (questionResult != null)
                    {
                        if ((questionResult.SelectedOption == option.OptionValue && questionResult.IsCorrect) || option.OptionValue == questionResult.CorrectAnswer)
                            return "correct";
                        else if (questionResult.SelectedOption == option.OptionValue && !questionResult.IsCorrect)
                            return "incorrect";
                    }
                }
            }

            return "";
        }

        public bool IsStartedOrFinishedChapter(ChapterModel chapter, QuizModel quiz)
        {
            if (quiz.QuizResult != null)
            {
                if (quiz.QuizResult.ChapterResults.Any(x => x.ChapterId == chapter.ChapterId))
                    return true;

                else if (chapter != null)
                {
                    return chapter.IsStarted || chapter.IsFinished;
                }
            }
            else if (chapter != null)
            {
                return chapter.IsStarted || chapter.IsFinished;
            }

            return false;
        }

        public QuizModel UpdateSelectedOptionState(string selectedOption, QuizModel quiz)
        {
            var optionToDeselect = quiz.CurrentQuestion!.Options.FirstOrDefault(o => o.IsSelected);
            if (optionToDeselect != null)
            {
                optionToDeselect.IsSelected = false;
            }

            var optionToSelect = quiz.CurrentQuestion.Options.FirstOrDefault(o => o.OptionValue == selectedOption);
            if (optionToSelect != null)
            {
                optionToSelect.IsSelected = !optionToSelect.IsSelected;
            }

            quiz.CurrentQuestion.IsAnswerd = true;

            return quiz;
        }

        public QuizModel UpdateCompletedChapterState(QuizModel quiz)
        {
            quiz.CurrentChapter!.IsFinished = true;
            quiz.CurrentChapter.IsStarted = false;
            quiz.CurrentQuestion = null;
            quiz.Display = Display.ChapterSummary;

            if (!_quizService.IsCompletedQuiz(quiz))
                quiz.NextAvailableChapter = quiz.Chapters[quiz.Chapters.IndexOf(quiz.CurrentChapter) + 1];
            else
                quiz.NextAvailableChapter = null;

            return quiz;
        }

        public QuizModel UpdateCompletedQuizState(QuizModel quiz)
        {
            quiz.NextAvailableChapter = null;
            quiz.CurrentChapter = null;
            quiz.CurrentQuestion = null;
            quiz.IsCompleted = true;
            quiz.Display = Display.QuizSummary;

            return quiz;
        }
    }
}
