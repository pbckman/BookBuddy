using Newtonsoft.Json;

namespace BookBuddy.Models.DataModels;

public class BookMetadata
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("authors")]
    public List<AuthorModel> Authors { get; set; } = [];

    [JsonProperty("plot")]
    public string Plot { get; set; } = string.Empty;

    [JsonProperty("formats")]
    public FormatModel Links { get; set; }

}
public class FormatModel
{
    [JsonProperty("image/jpeg")]
    public string ImageUrl { get; set; }
}
public class AuthorModel
{
    [JsonProperty("name")]
    public string Name { get; set; }
}
