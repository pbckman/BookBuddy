using BookBuddy.Models.Pages;
using static BookBuddy.Globals;

namespace BookBuddy.Models.Pages;

[ContentType(
GUID = "FFA9ED7B-01C1-45DA-904B-A550BA518340",
GroupName = GroupNames.Specialized
)]
public class SignUpPage : SitePageData
{
    [CultureSpecific]
    public virtual ContentReference? SiteSettingsPage { get; set; }
}



