using EPiServer.Web;
using System.ComponentModel.DataAnnotations;
using static BookBuddy.Globals;

namespace BookBuddy.Models.Pages
{
    [ContentType(
    GUID = "7BAFE0FC-2C85-44E7-83D3-F96A206474A1", GroupName = GroupNames.Specialized, DisplayName = "Slideshow", Description = "This is a slideshow template")]

    public class SlideShowPage : SitePageData
    {
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference Image { get; set; }

        [CultureSpecific]
        [Display(Name = "Title", Order = 20)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Name = "Description", Order = 30)]
        public virtual string Description { get; set; }

        [CultureSpecific]
        [Display(Name = "Button Text", Order = 40)]
        public virtual string ButtonText { get; set; }
    }
}
