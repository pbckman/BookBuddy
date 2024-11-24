using BookBuddy.Models.AchievementModels;

namespace BookBuddy.Business.Services.AchievementService
{
    public interface IAchievementService
    {
        Task<List<Achievement>> GetAchievementsAsync(int profileId, string lang);
        Task<List<Achievement>> GetUnFinishedAchievementsAsync(int profileId, string lang);
    }
}