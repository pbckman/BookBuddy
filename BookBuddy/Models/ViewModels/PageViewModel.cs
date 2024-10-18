using BookBuddy.Models.Pages;

namespace BookBuddy.Models.ViewModels
{
    public class PageViewModel<T> : IPageViewModel<T> where T : SitePageData
    {
        public PageViewModel(T currentPage) 
        {
            CurrentPage = currentPage;
        }
        public T CurrentPage { get; set; }
        public LayoutModel Layout { get; set; }
    }

    public static class PageViewModel
    {
        public static PageViewModel<T> Create<T>(T page) where T : SitePageData => new PageViewModel<T>(page);
    }
}
