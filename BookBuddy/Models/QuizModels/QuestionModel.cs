using Newtonsoft.Json;

namespace BookBuddy.Models.QuizModels
{
    public class QuestionModel
    {
        public int QuestionId { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionTitle { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public bool IsAnswerd { get; set; } = false;
        public List<OptionModel> Options { get; set; } = [];
        public string CorrectAnswer { get; set; } = string.Empty;


    }
}