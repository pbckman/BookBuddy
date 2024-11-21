namespace BookBuddy.Models.QuizModels
{
    public class QuizCardModel
    {
        public int QuizId { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ImageAltText { get; set; } = string.Empty;
        //public string PageUrl { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
