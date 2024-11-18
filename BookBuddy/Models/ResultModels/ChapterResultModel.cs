using BookBuddy.Data.Entities;

namespace BookBuddy.Models.ResultModels
{
    public class ChapterResultModel
    {
        public int QuizId { get; set; }
        public int ChapterId { get; set; }
        public List<QuestionResultModel> QuestionResults { get; set; } = [];
    }
}
