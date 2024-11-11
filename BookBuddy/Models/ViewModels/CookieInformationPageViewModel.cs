using BookBuddy.Models.Pages;

namespace BookBuddy.Models.ViewModels
{
    public class CookieInformationPageViewModel : PageViewModel<CookieInfoPage>
    {
        public CookieInformationPageViewModel(CookieInfoPage currentPage, SiteSettingsPage siteSettings) : base(currentPage, siteSettings)
        {

        }

    }
}
