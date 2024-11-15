using BookBuddy.Data.Entities;

namespace BookBuddy.Models.ResultModels
{
    public class QuestionResultModel
    {
        public int QuestionId { get; set; }
        public string SelectedOption { get; set; }
        public string CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }

    }
}
