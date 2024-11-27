using BookBuddy.Models.Pages;
using BookBuddy.Models.ResultModels;
using BootstrapBlazor.Components;

namespace BookBuddy.Models.ViewModels
{
    public class BookPageViewModel : PageViewModel<BookPage>
    {
        public BookPageViewModel(BookPage currentPage, SiteSettingsPage siteSettings) : base(currentPage, siteSettings)
        {
            BookId = currentPage.BookId;
            Title = currentPage.Title;
            Authors = currentPage.Authors;
            Plot = currentPage.Plot;
            ImageUrl = currentPage.ImageUrl;
            ImageAltText = currentPage.ImageAltText;
        }


        public int BookId { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Plot { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAltText { get; set; }
        public ResultStatus Status { get; set; }

    }
}
