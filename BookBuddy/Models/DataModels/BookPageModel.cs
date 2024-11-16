using System;

namespace BookBuddy.Models.DataModels;

public class BookPageModel
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Authors { get; set; }
    public string Plot { get; set; } = string.Empty;
    public string ImageUrl { get; set; }
    public string ImageAltText { get; set; }
    public string PageUrl { get; set; }
    public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
}
