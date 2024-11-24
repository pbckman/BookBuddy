using System;
using BookBuddy.Models.MyQuizzesModels;
using BookBuddy.Models.ResultModels;

namespace BookBuddy.Business.Services.MyQuizzesService;

public interface IMyQuizzesService
{
    Task<List<MyQuizzesModel>>? GetMyQuizzesAsync(int ProfileId, string Language);
    Task<MyQuizzesModel>? GetMyQuizzesDataAzync(int quizPageId, string lang, int profileId, QuizResultModel result);
}
