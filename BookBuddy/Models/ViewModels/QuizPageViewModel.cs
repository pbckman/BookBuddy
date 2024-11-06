using BookBuddy.Models.Pages;

namespace BookBuddy.Models.ViewModels
{
    public class QuizPageViewModel(QuizPage currentPage, SiteSettingsPage siteSettingsPage) : PageViewModel<QuizPage>(currentPage, siteSettingsPage)
    {
    }
}
