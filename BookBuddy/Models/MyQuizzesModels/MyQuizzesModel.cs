using System;
using BookBuddy.Models.QuizModels;
using BookBuddy.Models.ResultModels;

namespace BookBuddy.Models.MyQuizzesModels;

public class MyQuizzesModel
{
    public int QuizId { get; set; }
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string ImageAltText { get; set; } = string.Empty;
    public string PageUrl { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public int TotalCorrectAnswers { get; set; }
    public int TotalWrongAnswers { get; set; }
    public int TotalQuestions { get; set; }
    public int TotalChapters { get; set; }
    //CMS 
    public List<ChapterModel> Chapters { get; set; } = new List<ChapterModel>();
    
    // await ProfileService.GetSelectedProfileAsync();
    // await QuizResultService.GetResultsByProfileIdAsync(profile.Id); ???
    // await QuizService.GetActiveQuizzesAsync(results, Language);
    public QuizResultModel? QuizResult { get; set; }

}
