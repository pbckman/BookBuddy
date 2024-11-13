using EPiServer.Web;
using System.ComponentModel.DataAnnotations;
using static BookBuddy.Globals;

namespace BookBuddy.Models.Pages
{
    [ContentType(
        DisplayName = "CookieInformationPage",
        GUID = "4244B252-60B8-4F01-B418-C83D305571CD",
        GroupName = GroupNames.Specialized,
        Description ="This is a cookie page"
    )]
    public class CookieInfoPage : SitePageData
    {
        [CultureSpecific]
        public virtual ContentReference? SiteSettingsPage { get; set; }
    }
}
