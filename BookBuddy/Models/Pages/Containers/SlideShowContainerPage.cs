namespace BookBuddy.Models.Pages.Containers
{
    [ContentType(GUID = "88CA7A73-213B-47E9-B5CA-07678125C858", DisplayName = "SlideShowContainer")]
    [AvailableContentTypes(Availability.Specific, Include = [typeof(SlideShowPage)])]
    public class SlideShowContainerPage : PageData, IContainerPage
    {
    }
}
