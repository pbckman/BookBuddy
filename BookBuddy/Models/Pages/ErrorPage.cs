using static BookBuddy.Globals;

namespace BookBuddy.Models.Pages
{
    [ContentType(
   GUID = "0D6CF6A9-D993-4AD9-BD58-AFB782EC0CB6",
   GroupName = GroupNames.Specialized
   )]
    public class ErrorPage : SitePageData
    {
        [CultureSpecific]
        public virtual ContentReference? SiteSettingsPage { get; set; }
    }
}
