using BookBuddy.Models.DataModels;

namespace BookBuddy.Business.Services.AiService
{
    public interface IAiService
    {
        Task<QuizChapterModel> GenerateQuizQuestionsAsync(string chapterContent, string bookTitle, string lang);
        Task<MetadataDetails> GenerateMetadataAsync(string bookContent, string lang);
    }
}