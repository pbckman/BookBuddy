using BookBuddy.Business.Factories;
using EPiServer.Shell.ObjectEditing;
using System.ComponentModel.DataAnnotations;
using static BookBuddy.Globals;

namespace BookBuddy.Models.Pages;

[ContentType(
 GroupName = GroupNames.Specialized,
 GUID = "DBB457FA-4F76-475B-951D-0294CD67CA82"
)]
[ImageUrl("/pages/CMS-icon-page-02.png")]

public class AvailableBooksPage : SitePageData
{
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 10,
        Name = "Available Books"
    )]
    [CultureSpecific]
    [SelectOne(SelectionFactoryType = typeof(BookSelectionFactory))]
    public virtual string SelectedBook { get; set; } = string.Empty;
}
