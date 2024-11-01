namespace BookBuddy.Models.Pages.Containers
{
    [ContentType(
        GUID = "BC9BC72E-32A7-45D3-BB55-4B2CB793BCDB"
    )]
    [AvailableContentTypes(
        Availability.Specific,
        Include =
            [typeof(BookPage)
    ]

    )]
    public class LanguageContainerPage : PageData, IContainerPage
    {
    }
}
