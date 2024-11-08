namespace BookBuddy.Data.Entities
{
    public class ChapterResultEntity
    {
        public int Id { get; set; }
        public int ChapterId { get; set; }
        public List<QuestionResultEntity> QuestionResults { get; set; } = [];
        public int QuizResultId { get; set; }
        public QuizResultEntity QuizResult { get; set; }
    }
}
