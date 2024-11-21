using System.ComponentModel.DataAnnotations;
using static BookBuddy.Globals;
namespace BookBuddy.Models.Pages
{
    [ContentType(
        GUID = "CF3E7300-3534-4D4B-A69F-3759CE7F9575",
        GroupName = GroupNames.Specialized,
        Description = "An example page"
        )]
    public class ExamplePage : SitePageData
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 10
        )]
        [CultureSpecific]
        public virtual string Heading { get; set; } = string.Empty;
    }
}