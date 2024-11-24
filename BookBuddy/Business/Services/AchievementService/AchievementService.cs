using BookBuddy.Business.Services.PageService;
using BookBuddy.Business.Services.QuizResultService;
using BookBuddy.Models.AchievementModels;
using BookBuddy.Models.Blocks;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ResultModels;
using EPiServer.Shell;

namespace BookBuddy.Business.Services.AchievementService
{
    public class AchievementService : IAchievementService
    {
        private readonly IPageService _pageService;
        private readonly IContentLoader _contentLoader;
        private readonly IQuizResultService _quizResultService;


        public AchievementService(IPageService pageService, IContentLoader contentLoader, IQuizResultService quizResultService)
        {
            _pageService = pageService;
            _contentLoader = contentLoader;
            _quizResultService = quizResultService;
        }

        public async Task<List<Achievement>> GetAchievementsAsync(int profileId, string lang)
        {
            var allAchievements = GetAchievements(lang);
            var results = await _quizResultService.GetResultsAsync(profileId, lang);

            var achievementsByProfile = GetProfileAchievements(allAchievements, results);

            if(achievementsByProfile != null)
                return achievementsByProfile.Select(achievement => new Achievement
                {
                    Name = achievement.Name,
                    ImageUrl = achievement.ImageUrl,
                    ImageAltText = achievement.ImageAltText
                }).ToList();
            else
                return new List<Achievement>();
        }

        private List<AchievementModel> GetProfileAchievements(List<AchievementModel> allAchievements, List<QuizResultModel> results)
        {
            var achievements = new List<AchievementModel>();
            foreach (var achievement in allAchievements)
            {
                if(achievement.AmountOfChapters > 0 && achievement.AmountOfBooks == 0 && achievement.CorrectPercent == 0)
                {
                    if(results.Sum(x => x.ChapterResults.Count) >= achievement.AmountOfChapters)
                        achievements.Add(achievement);
                }
                else if (achievement.AmountOfChapters == 0 && achievement.AmountOfBooks > 0 && achievement.CorrectPercent == 0)
                {
                    if (results.Count(x => x.IsCompleted) >= achievement.AmountOfBooks)
                        achievements.Add(achievement);
                }
                else if(achievement.AmountOfChapters > 0 && achievement.AmountOfBooks > 0 && achievement.CorrectPercent == 0)
                {
                    if(results.Count(x => x.IsCompleted) >= achievement.AmountOfBooks && results.Sum(x => x.ChapterResults.Count) >= achievement.AmountOfChapters)
                        achievements.Add(achievement);
                }
                else if(achievement.AmountOfChapters > 0 && achievement.AmountOfBooks == 0 && achievement.CorrectPercent > 0)
                {
                    if (results.Count(x => x.ChapterResults.Any(x => IsMatchingPercent(x, achievement.CorrectPercent))) >= achievement.AmountOfChapters)
                        achievements.Add(achievement);
                }
                else if(achievement.AmountOfChapters == 0 && achievement.AmountOfBooks > 0 && achievement.CorrectPercent > 0)
                {
                    if(results.Count(x => IsMatchingPercent(x, achievement.CorrectPercent)) >= achievement.AmountOfBooks)
                        achievements.Add(achievement);
                }
                else if(achievement.AmountOfChapters > 0 && achievement.AmountOfBooks > 0 && achievement.CorrectPercent > 0)
                {
                    if(results.Count(x => IsMatchingPercent(x, achievement.CorrectPercent)) >= achievement.AmountOfBooks &&
                       results.Count(x => x.ChapterResults.Any(x => IsMatchingPercent(x, achievement.CorrectPercent))) >= achievement.AmountOfChapters)
                        achievements.Add(achievement);
                }
            }

            return achievements;
        }

        private bool IsMatchingPercent(ChapterResultModel chapterResult, int correctPercent)
        {
            decimal correctAnswers = chapterResult.QuestionResults.Count(x => x.IsCorrect);
            decimal totalAnswers = chapterResult.QuestionResults.Count;
            decimal percent = (correctAnswers / totalAnswers) * 100;

            return percent >= correctPercent;
        }

        private bool IsMatchingPercent(QuizResultModel quizResult, int correctPercent)
        {
            decimal correctAnswers = quizResult.ChapterResults.Sum(chapterResult => chapterResult.QuestionResults.Count(x => x.IsCorrect));
            decimal totalAnswers = quizResult.ChapterResults.Sum(chapterResult => chapterResult.QuestionResults.Count);
            decimal percent = (correctAnswers / totalAnswers) * 100;

            return percent >= correctPercent;
        }

        private List<AchievementModel> GetAchievements(string lang)
        {
            var achievementsPage = _pageService.GetAchievementsPage(lang);
            if (achievementsPage == null)
            {
                return new List<AchievementModel>();
            }

            var achievements = new List<AchievementBlock>();

            if (achievementsPage.Achievements != null)
            {
                foreach (var item in achievementsPage.Achievements.Items)
                {
                    if (_contentLoader.TryGet(item.ContentLink, new LanguageSelector(lang), out AchievementBlock achievementBlock))
                    {
                        achievements.Add(achievementBlock);
                    }
                }
            }

            return achievements.Select(achievement => new AchievementModel
            {
                Name = achievement.Title,
                ImageUrl = achievement.Image.GetUri().ToString(),
                ImageAltText = achievement.ImageAltText,
                AmountOfBooks = achievement.NumberOfBooks,
                AmountOfChapters = achievement.NumberOfChapters,
                CorrectPercent = achievement.Percentage
            }).ToList();
        }

        public async Task<List<Achievement>> GetUnFinishedAchievementsAsync(int profileId, string lang)
        {
            var allAchievements = GetAchievements(lang);
            var profileAchievements = await GetAchievementsAsync(profileId, lang);

            var unFinishedAchievements = allAchievements.Where(x => !profileAchievements.Any(y => y.Name == x.Name)).Select(achievement => new Achievement
            {
                Name = achievement.Name,
                ImageUrl = achievement.ImageUrl,
                ImageAltText = achievement.ImageAltText
            }).ToList();

            if(unFinishedAchievements != null)
                return unFinishedAchievements;
            else
                return new List<Achievement>();

        }
    }
}
