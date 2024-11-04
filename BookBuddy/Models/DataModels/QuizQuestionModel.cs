using Newtonsoft.Json;

namespace BookBuddy.Models.DataModels
{
    public class QuizQuestionModel
    {
        [JsonProperty("question_number")]
        public int QuestionNumber { get; set; }

        [JsonProperty("question")]
        public string Question { get; set; }

        [JsonProperty("options")]
        public Dictionary<string, string> Options { get; set; }

        [JsonProperty("correct_answer")]
        public string CorrectAnswer { get; set; }
    }
}
