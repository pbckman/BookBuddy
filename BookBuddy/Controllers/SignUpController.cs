using BookBuddy.Models.Pages;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
    public class SignUpController : PageControllerBase<SignUpPage>
    {
        [Route("/SignUp")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
