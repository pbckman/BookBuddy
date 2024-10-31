using BookBuddy.Models.Pages;

namespace BookBuddy.Models.ViewModels
{
    public class BasicPageViewModel<T> : IBasicPageViewModel<T> where T : SitePageData
    {
        public BasicPageViewModel(T currentPage) 
        {
            CurrentPage = currentPage;
        }
        public T CurrentPage { get; set; }
        public LayoutModel Layout { get; set; }

    }

    public static class BasicPageViewModel
    {
        public static BasicPageViewModel<T> Create<T>(T page) where T : SitePageData => new BasicPageViewModel<T>(page);
    }
}
