using EPiServer.Framework.DataAnnotations;

namespace BookBuddy.Models
{
    [ContentType(
        GUID = "A777ECF8-5E80-42BC-A224-1458AD8D5F49")]

    [MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png,webp,pdf,jfif,svg")]
    public class ImageFile : ImageData
    {
    }
}
