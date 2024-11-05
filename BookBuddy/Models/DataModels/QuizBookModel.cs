namespace BookBuddy.Models.DataModels
{
    public class QuizBookModel
    {
        public BookMetadata Metadata { get; set; }
        public List<QuizChapterModel> Chapters { get; set; } = [];
    }
}
