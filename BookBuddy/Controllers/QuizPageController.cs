using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
    public class QuizPageController : PageControllerBase<QuizPage>
    {
 
        public IActionResult Index(QuizPage currentPage)
        {
            var model = new QuizPageViewModel(currentPage, null);

        

            return View(model);
        }
    }
}
