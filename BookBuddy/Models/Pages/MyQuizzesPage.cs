using System.ComponentModel.DataAnnotations;
using static BookBuddy.Globals;

namespace BookBuddy.Models.Pages;

[ContentType(
    GUID = "108A1F4E-4769-4482-A717-979B90D99316",
    GroupName = GroupNames.Specialized
)]

public class MyQuizzesPage : SitePageData
{
    [Display(
    GroupName = SystemTabNames.Content,
    Order = 10,
    Name = "Heading Ongoing"
    )]
    [CultureSpecific]
    public virtual string HeadingOnGoing { get; set; } = string.Empty;

    [Display(
    GroupName = SystemTabNames.Content,
    Order = 20,
    Name = "Heading Finished"
    )]
    [CultureSpecific]
    public virtual string HeadingFinished { get; set; } = string.Empty;

    [Display(
       GroupName = SystemTabNames.Content,
       Order = 30
    )]
    [CultureSpecific]
    public virtual ContentReference? SiteSettingsPage { get; set; }
}


