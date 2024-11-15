namespace BookBuddy.Models.QuizModels
{
    public class LSQuizState
    {
        public int ChapterId { get; set; }
        public List<LSQuestionModel> AnsweredQuestions { get; set; } = [];
    }
    public class LSQuestionModel
    {
        public int QuestionId { get; set; }
        public string SelectedOption { get; set; } = null!;
    }
}
