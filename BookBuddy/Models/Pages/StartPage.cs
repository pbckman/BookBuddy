using System.ComponentModel.DataAnnotations;
using static BookBuddy.Globals;

namespace BookBuddy.Models.Pages
{
    [ContentType(
    GUID = "F7A0B723-F6D5-498D-AAAB-69B7C69D19A2",
    GroupName = GroupNames.Specialized
    )]
    [AvailableContentTypes(Availability.Specific, Include =
    [
        typeof(ErrorPage),
        typeof(XmlSitemap),
        typeof(BooksPage),
        typeof(MyQuizzesPage)
        typeof(BooksPage),
        typeof(AvailableBooksPage),
        typeof(ExamplePage),
        typeof(CookieInfoPage),
        typeof(SiteSettingsPage)

    ])]

    public class StartPage : SitePageData
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 10
        )]
        [CultureSpecific]
        public virtual string Heading { get; set; } = string.Empty;

        [Display(
           GroupName = SystemTabNames.Content,
           Order = 20
        )]
        [CultureSpecific]
        public virtual XhtmlString? MainBody { get; set; }

        [Display(
           GroupName = SystemTabNames.Content,
           Order = 30
        )]
        [CultureSpecific]
        public virtual ContentReference? SiteSettingsPage { get; set; }
    }
}
