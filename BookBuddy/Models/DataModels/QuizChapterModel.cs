using Newtonsoft.Json;

namespace BookBuddy.Models.DataModels
{
    public class QuizChapterModel
    {
        [JsonProperty("chapter_title")]
        public string ChapterTitle { get; set; }

        [JsonProperty("questions")]
        public List<QuizQuestionModel> Questions { get; set; } = [];
    }
}
