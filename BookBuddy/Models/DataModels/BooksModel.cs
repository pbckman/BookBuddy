namespace BookBuddy.Models.DataModels
{
    public class BooksModel
    {
        public int ResultCount { get; set; }
        public List<BookPageModel> Books { get; set; } = new List<BookPageModel>();
    }
}
