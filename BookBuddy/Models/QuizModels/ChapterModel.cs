namespace BookBuddy.Models.QuizModels
{
    public class ChapterModel
    {
        public int ChapterId { get; set; }
        public string ChapterTitle { get; set; }
        public bool ShowQuestions { get; set; } = false;
        public List<QuestionModel> Questions { get; set; } = [];
    }
}
