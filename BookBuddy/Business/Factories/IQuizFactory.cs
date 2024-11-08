using BookBuddy.Data.Entities;
using BookBuddy.Models.Pages;
using BookBuddy.Models.QuizModels;
using BookBuddy.Models.ResultModels;

namespace BookBuddy.Business.Factories
{
    public interface IQuizFactory
    {
        QuizModel Create(QuizPage currentPage);
        QuizResultModel Create(QuizResultEntity result);
    }
}
