using static BookBuddy.Globals;

namespace BookBuddy.Models.Pages
{
    [ContentType(
        GroupName = GroupNames.Specialized,
        GUID = "1224E217-8237-4F99-A400-75A089EB1BD3"
    )]
    [ImageUrl("/pages/CMS-icon-page-02.png")]
    [AvailableContentTypes(
         Availability.Specific,
         Include =
         [
             typeof(QuestionPage)
         ]
     )]
    public class ChapterPage : SitePageData
    {
    }
}
