using BookBuddy.Models.Pages;

namespace BookBuddy.Models.ViewModels
{
    public class PageViewModel<T> : IPageViewModel<T> where T : SitePageData
    {
        public PageViewModel(T currentPage, SiteSettingsPage siteSettings) 
        {
            CurrentPage = currentPage;
            SiteSettings = siteSettings;
        }
        public T CurrentPage { get; set; }
        public LayoutModel Layout { get; set; }

        public SiteSettingsPage SiteSettings { get; }
    }

    public static class PageViewModel
    {
        public static PageViewModel<T> Create<T>(T page, SiteSettingsPage siteSettings) where T : SitePageData => new PageViewModel<T>(page, siteSettings);
    }
}
