using BookBuddy.Business.Services.AuthorizedService;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
 
    public class QuizPageController(IAuthorizedService authorizedService) : PageControllerBase<QuizPage>
    {
        private readonly IAuthorizedService _authorizedService = authorizedService;

        public IActionResult Index(QuizPage currentPage)
        {
            if (!_authorizedService.IsUserAuthorizedAsync().Result)
                {
                    System.Diagnostics.Debug.WriteLine("User is not authorized");
                    return RedirectToAction("Index", "StartPage");
                }

            var model = new QuizPageViewModel(currentPage);
           
            return View(model);
        }
    }
}
