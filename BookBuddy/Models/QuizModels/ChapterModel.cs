namespace BookBuddy.Models.QuizModels
{
    public class ChapterModel
    {
        public int ChapterId { get; set; }
        public string ChapterTitle { get; set; } = string.Empty;
        public bool ShowQuestions { get; set; } = false;
        public bool IsFinished { get; set; } = false;
        public bool IsStarted { get; set; } = false;
        public bool ShowIntro { get; set; } = false;

        public ChapterSelectionSummaryModel? ChapterSelectionSummary { get; set; }
        public List<QuestionModel> Questions { get; set; } = [];
    }
}
