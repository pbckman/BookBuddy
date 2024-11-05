using System;
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
            typeof(BookPage)
            ]
   )]
    public class BooksPage : SitePageData
    {
    }
