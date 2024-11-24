using System;
using System.ComponentModel.DataAnnotations;
using static BookBuddy.Globals;

namespace BookBuddy.Models.Pages;

    [ContentType(
        GUID = "8ec81601-2f73-47f0-9cb1-cc68cb571a1e",
        GroupName = GroupNames.Specialized
   )]
    [AvailableContentTypes(
       Availability.Specific,
       Include =
           [
            typeof(AvailableBooksPage),
            typeof(AchievementsPage),
            typeof(BookPage)
            ]
   )]
    public class BooksPage : SitePageData
    {
        [Display(
           GroupName = SystemTabNames.Content,
           Order = 30
        )]
        [CultureSpecific]
        public virtual ContentReference? SiteSettingsPage { get; set; }
    }
