using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers
{
    public class ExampleController : PageControllerBase<ExamplePage>
    {
        public IActionResult Index(ExamplePage currentPage)
        {
            var model = new ExamplePageViewModel(currentPage);
            return View(model);
        }
    }
}
