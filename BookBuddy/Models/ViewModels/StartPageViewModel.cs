using BookBuddy.Models.Pages;

namespace BookBuddy.Models.ViewModels
{
    public class StartPageViewModel(StartPage currentPage, SiteSettingsPage siteSettings) : PageViewModel<StartPage>(currentPage, siteSettings)
    {
    }
}
