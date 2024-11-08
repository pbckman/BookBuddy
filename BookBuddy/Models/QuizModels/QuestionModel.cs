using Newtonsoft.Json;

namespace BookBuddy.Models.QuizModels
{
    public class QuestionModel
    {
        public int QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public string Question { get; set; }
        public Dictionary<string, string> Options { get; set; }
        public string CorrectAnswer { get; set; }
    }
}