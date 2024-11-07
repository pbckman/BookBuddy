using static BookBuddy.Globals;
using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Models.Pages
{
    [ContentType(
    GroupName = GroupNames.Specialized,
    GUID = "315D43CD-E4BD-47DB-98A6-621E31CC78D2"
)]
    [ImageUrl("/pages/CMS-icon-page-02.png")]
    [AvailableContentTypes(
        Availability.Specific,
        Include =
        [
            typeof(QuizPage)
        ]
    )]
    public class BookPage : SitePageData
    {
        [Display(
                GroupName = SystemTabNames.Content,
                Order = 10,
                Name = "Book Id"
            )]
        [CultureSpecific]
        [Editable(false)]
        public virtual int BookId { get; set; }

        [Display(
                GroupName = SystemTabNames.Content,
                Order = 20,
                Name = "Title"
            )]
        [CultureSpecific]
        public virtual string Title { get; set; }

        [Display(
               GroupName = SystemTabNames.Content,
               Order = 30,
               Name = "Authors"
           )]
        [CultureSpecific]
        public virtual string Authors { get; set; }

        [Display(
             GroupName = SystemTabNames.Content,
             Order = 40,
             Name = "Plot"
         )]
        [CultureSpecific]
        public virtual string Plot { get; set; }

        [Display(
             GroupName = SystemTabNames.Content,
             Order = 50,
             Name = "Image url"
         )]
        [CultureSpecific]
        public virtual string ImageUrl { get; set; }

        [Display(
       GroupName = SystemTabNames.Content,
       Order = 60,
       Name = "Image alt text"
   )]
        [CultureSpecific]
        public virtual string ImageAltText { get; set; }


        [CultureSpecific]
        public virtual ContentReference? SiteSettingsPage { get; set; }
    }


}
