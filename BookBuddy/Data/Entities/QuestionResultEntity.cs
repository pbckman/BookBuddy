namespace BookBuddy.Data.Entities
{
    public class QuestionResultEntity
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string SelectedOption { get; set; }
        public bool IsCorrect { get; set; }
        public string CorrectAnswer { get; set; }

        public int ChapterResultId { get; set; }
        public ChapterResultEntity ChapterResult { get; set; }
    }
}