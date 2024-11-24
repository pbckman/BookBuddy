using BookBuddy.Business.Factories;
using BookBuddy.Models.Blocks;
using EPiServer.Shell.ObjectEditing;
using System.ComponentModel.DataAnnotations;
using static BookBuddy.Globals;

namespace BookBuddy.Models.Pages;

[ContentType(
    DisplayName = "Achievements Page",
    GUID = "7B951B9A-7BBE-42BF-8152-6FFB9D84EE66",
    GroupName = GroupNames.Specialized
)]

public class AchievementsPage : SitePageData
{
    [Display(
            GroupName = SystemTabNames.Content,
            Order = 10)]
    [AllowedTypes(typeof(AchievementBlock))]
    [CultureSpecific]
    public virtual ContentArea Achievements { get; set; }

}













