namespace BookBuddy.Models.AchievementModels
{
    public class AchievementModel
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ImageAltText { get; set; } = string.Empty;
        public int AmountOfBooks { get; set; }
        public int AmountOfChapters { get; set; }
        public int CorrectPercent { get; set; }
    }
}
