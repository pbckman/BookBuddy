
using BookBuddy.Models.DataModels;
using BookBuddy.Models.Pages;

namespace BookBuddy.Models.ViewModels
{
    public class StartPageViewModel(StartPage currentPage, SiteSettingsPage siteSettings) : PageViewModel<StartPage>(currentPage, siteSettings)
    {
        public HeroSectionModel? HeroSectionModel { get; set; }

        public InfoSectionModel? InfoSectionModel { get; set; }
        public List<BookPageModel>? Books { get; set; }
        
    }


}
