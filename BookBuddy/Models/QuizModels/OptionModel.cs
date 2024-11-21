namespace BookBuddy.Models.QuizModels
{
    public class OptionModel
    {
        public string OptionValue { get; set; } = string.Empty;
        public string OptionText { get; set; } = string.Empty;
        public bool IsSelected { get; set; } = false;
    }
}