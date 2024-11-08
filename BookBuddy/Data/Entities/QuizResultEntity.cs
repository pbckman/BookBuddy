namespace BookBuddy.Data.Entities
{
    public class QuizResultEntity
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public List<ChapterResultEntity> ChapterResults { get; set; } = [];
        public int ProfileId { get; set; }
        public UserProfileEntity Profile { get; set; }
    }
}
