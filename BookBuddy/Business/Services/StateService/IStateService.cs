using BookBuddy.Models.QuizModels;
using BootstrapBlazor.Components;

namespace BookBuddy.Business.Services.StateService
{
    public interface IStateService
    {
        QuizModel UpdateSelectedQuestionState(ChapterModel chapter, QuestionModel question, QuizModel quiz);
        QuizModel UpdateSelectedChapterState(ChapterModel chapter, QuizModel quiz);
        QuizModel UpdateCurrentStateByQuestionId(int questionId, QuizModel quiz);
        QuizModel UpdateSelectedOptionState(string selectedOption, QuizModel quiz);
        QuizModel UpdateCompletedChapterState(QuizModel quiz);
        QuizModel UpdateCompletedQuizState(QuizModel quiz);
        bool IsFinishedChapter(ChapterModel chapter, QuizModel quiz);
        bool IsAvailableChapter(ChapterModel chapter, QuizModel quiz);
        bool IsStartedOrFinishedChapter(ChapterModel chapter, QuizModel quiz);
        QuizModel UpdateSelectedSummaryState(ChapterModel chapter, QuizModel quiz);
        string GetQuestionState(QuestionModel question, QuizModel quiz);
        string GetOptionState(OptionModel option, QuizModel quiz);
        QuizModel SetQuizStartState(QuizModel quiz);

    }
}