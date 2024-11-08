namespace BookBuddy.Models.QuizModels
{
    public class QuizModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAltText { get; set; }
        public List<ChapterModel> Chapters { get; set; } = [];
    }
}
