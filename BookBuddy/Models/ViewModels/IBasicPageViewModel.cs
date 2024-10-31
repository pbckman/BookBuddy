using BookBuddy.Models.Pages;

namespace BookBuddy.Models.ViewModels
{
    public interface IBasicPageViewModel<out T> where T : SitePageData
    {
        T CurrentPage { get; }

        LayoutModel Layout { get; set; }
    }
}
