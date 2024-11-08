using BookBuddy.Data.Entities;

namespace BookBuddy.Models.ResultModels
{
    public class QuizResultModel
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public List<ChapterResultModel> ChapterResults { get; set; } = [];
    }
}
