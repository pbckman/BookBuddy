using BookBuddy.Business.Factories;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
 
    public class QuizPageController : PageControllerBase<QuizPage>
    {
        private readonly IQuizFactory _quizFactory;

        public QuizPageController(IQuizFactory quizFactory)
        {
            _quizFactory = quizFactory;
        }

        public IActionResult Index(QuizPage currentPage)
        {
            var model = new QuizPageViewModel(currentPage);
           

            return View(model);
        }
    }
}
