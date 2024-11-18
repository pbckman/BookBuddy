using BookBuddy.Models.ResultModels;

namespace BookBuddy.Models.QuizModels
{
    public class QuizModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ImageAltText { get; set; } = string.Empty;
        public string NextQuestionBtnText { get; set; } = string.Empty;
        public string NextChapterBtnText { get; set; } = string.Empty;
        public string SubmitBtnText { get; set; } = string.Empty;
        public string StartBtnText { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public string QuizResultTitle { get; set; } = string.Empty;
        public Display Display { get; set; }
        public ChapterModel? CurrentChapter { get; set; }
        public QuestionModel? CurrentQuestion { get; set; }
        public ChapterModel? NextAvailableChapter { get; set; }
        public List<ChapterModel> Chapters { get; set; } = [];
        public QuizResultModel? QuizResult { get; set; }
    }
}
