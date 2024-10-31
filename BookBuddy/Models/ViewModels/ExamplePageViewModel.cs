using BookBuddy.Models.Pages;

namespace BookBuddy.Models.ViewModels
{
    public class ExamplePageViewModel(ExamplePage currentPage) : BasicPageViewModel<ExamplePage>(currentPage)
    {
    }
}
