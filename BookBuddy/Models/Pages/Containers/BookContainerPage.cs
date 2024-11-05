namespace BookBuddy.Models.Pages.Containers
{
    [ContentType(
       GUID = "8CEC7F59-72C1-44B5-8F9C-9DCF501243FE"
   )]
    [AvailableContentTypes(
       Availability.Specific,
       Include =
           [typeof(AvailableBooksPage)
        ]

   )]
    public class BookContainerPage : PageData, IContainerPage
    {
    }
}
