using System;
using BookBuddy.Business.Factories;
using BookBuddy.Business.Services.PageService;
using BookBuddy.Business.Services.QuizResultService;
using BookBuddy.Business.Services.QuizService;
using BookBuddy.Models.MyQuizzesModels;
using BookBuddy.Models.QuizModels;
using BookBuddy.Models.ResultModels;

namespace BookBuddy.Business.Services.MyQuizzesService;

public class MyQuizzesService : IMyQuizzesService
{
    // private readonly IPageService _pageService;
    // private readonly MyQuizzesFactory _myQuizzesFactory;
    // private readonly ILogger<MyQuizzesService> _logger;


    // public MyQuizzesService(IPageService pageService, MyQuizzesFactory myQuizzesFactory, ILogger<MyQuizzesService> logger)
    // {
    //     _pageService = pageService;
    //     _myQuizzesFactory = myQuizzesFactory;
    //     _logger = logger;
    // }
    private readonly IQuizService _quizService;
    private readonly IQuizResultService _quizResultService;
    private readonly ILogger<MyQuizzesService> _logger;
    private readonly IPageService _pageService;
    private readonly MyQuizzesFactory _myQuizzesFactory;

    public MyQuizzesService(IQuizService quizService, IQuizResultService quizResultService, ILogger<MyQuizzesService> logger, IPageService pageService, MyQuizzesFactory myQuizzesFactory)
    {
        _quizService = quizService;
        _quizResultService = quizResultService;
        _logger = logger;
        _pageService = pageService;
        _myQuizzesFactory = myQuizzesFactory;
    }

    public async Task<List<MyQuizzesModel>> GetMyQuizzesAsync(int ProfileId, string Language)
    {
        try
        {
            var results = await _quizResultService.GetResultsByProfileIdAsync(ProfileId);
            var quizzes = await _quizService.GetActiveQuizzesAsync(results, Language);
            var FilteredResults = results.Where(r => r != null && quizzes.Any(q => q.QuizId == r.QuizId)).ToList();
            var myQuizzesList = new List<MyQuizzesModel>();

            if (FilteredResults.Count == 0)
            {
                return new List<MyQuizzesModel>();
            }
            
            foreach (var result in FilteredResults)
            {
                if (result != null)
                {
                    var quizData = await GetMyQuizzesDataAzync(result.QuizId, Language, ProfileId, result);
                    if (quizData != null)
                    {
                        myQuizzesList.Add(quizData);
                    }
                }
            }

            return myQuizzesList ?? new List<MyQuizzesModel>();    
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR :  MyQuizzesService.GetMyQuizzesAsync() : {0}", ex.Message);
        }
        return new List<MyQuizzesModel>();
    }


    public async Task<MyQuizzesModel> GetMyQuizzesDataAzync(int quizPageId, string lang, int profileId, QuizResultModel result)
    {
        try
        {
            var currentQuizPage = await Task.Run(() => _pageService.GetQuizPageById(quizPageId, lang));
            if (currentQuizPage == null)
            {
                _logger.LogError("ERROR :  MyQuizzesService.GetMyQuizzesDataAzync() : QuizPage not found");
                return new MyQuizzesModel();
            }


            var quiz = _myQuizzesFactory.Create(currentQuizPage, result);
            return quiz ?? new MyQuizzesModel();    
            
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR :  MyQuizzesService.GetMyQuizzesDataAzync() : {0}", ex.Message);
        }
        return new MyQuizzesModel();
    }
}
