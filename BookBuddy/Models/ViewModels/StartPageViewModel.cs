using BookBuddy.Models.Pages;

namespace BookBuddy.Models.ViewModels
{
    public class StartPageViewModel(StartPage currentPage) : PageViewModel<StartPage>(currentPage)
    {
    }
}
