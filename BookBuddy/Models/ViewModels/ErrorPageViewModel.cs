using BookBuddy.Models.Pages;

namespace BookBuddy.Models.ViewModels
{
    public class ErrorPageViewModel(ErrorPage currentPage, SiteSettingsPage siteSettings) : PageViewModel<ErrorPage>(currentPage, siteSettings)
    {
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ErrorText { get; set; }
        public string? GoToHomeButtonText { get; set; }

    }
}
