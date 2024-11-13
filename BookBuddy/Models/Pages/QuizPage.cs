using System;
using System.ComponentModel.DataAnnotations;
using static BookBuddy.Globals;

namespace BookBuddy.Models.Pages;

[ContentType(
GroupName = GroupNames.Specialized,
GUID = "2d9a8029-e666-4cde-93f0-e1a114527137"
)]
    [ImageUrl("/pages/CMS-icon-page-02.png")]
    [AvailableContentTypes(
        Availability.Specific,
        Include =
        [
            typeof(ChapterPage)
        ]
)]
public class QuizPage : SitePageData
{
    [Display(
           GroupName = SystemTabNames.Content,
           Order = 30
        )]
        [CultureSpecific]
        public virtual ContentReference? SiteSettingsPage { get; set; }
}
