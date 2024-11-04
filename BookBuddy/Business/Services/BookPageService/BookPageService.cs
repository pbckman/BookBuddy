using BookBuddy.Business.Services.PageService;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.Pages;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.Web;
using System.Globalization;

namespace BookBuddy.Business.Services.BookPageService
{
    public class BookPageService : IBookPageService
    {
        private readonly IContentRepository _contentRepository;
        private readonly IContentLoader _contentLoader;
        private readonly ISiteDefinitionRepository _siteDefinitionRepository;
        private readonly IPageService _pageService;


        public BookPageService(IContentRepository contentRepository, IContentLoader contentLoader, ISiteDefinitionRepository siteDefinitionRepository, IPageService pageService)
        {
            _contentRepository = contentRepository;
            _contentLoader = contentLoader;
            _siteDefinitionRepository = siteDefinitionRepository;
            _pageService = pageService;
        }

        public bool CreatePages(QuizBookModel bookQuizModel, string selectedLanguage)
        {
            try
            {
                var languageContainerPage = _pageService.GetLanguageContainerPage(selectedLanguage);

                if (languageContainerPage == null)
                    return false;


                var bookPage = _contentRepository.GetDefault<BookPage>(languageContainerPage.ContentLink, CultureInfo.GetCultureInfo(selectedLanguage));
                bookPage.BookId = bookQuizModel.Metadata.Id;
                bookPage.Name = bookQuizModel.Metadata.Title;
                bookPage.Title = bookQuizModel.Metadata.Title;
                bookPage.Authors = string.Join(", ", bookQuizModel.Metadata.Authors.Select(x => x.Name));
                bookPage.Plot = bookQuizModel.Metadata.Plot;
                bookPage.ImageUrl = bookQuizModel.Metadata.Links.ImageUrl;
                bookPage.ImageAltText = selectedLanguage == "sv" ? $"Bokomslag för {bookQuizModel.Metadata.Title}" : $"Book cover for {bookQuizModel.Metadata.Title}";
                bookPage.ChildSortOrder = EPiServer.Filters.FilterSortOrder.CreatedAscending;

                var bookPageReference = _contentRepository.Save(bookPage, SaveAction.Publish, AccessLevel.NoAccess);


                foreach (var chapter in bookQuizModel.Chapters)
                {
                    var chapterPage = _contentRepository.GetDefault<ChapterPage>(bookPageReference, CultureInfo.GetCultureInfo(selectedLanguage));
                    chapterPage.Name = chapter.ChapterTitle;
                    chapterPage.ChildSortOrder = EPiServer.Filters.FilterSortOrder.CreatedAscending;

                    var chapterPageReference = _contentRepository.Save(chapterPage, SaveAction.Publish, AccessLevel.NoAccess);

                    var questionNamePrefix = selectedLanguage == "sv" ? "Fråga" : "Question";
                    foreach (var question in chapter.Questions)
                    {

                        var questionPage = _contentRepository.GetDefault<QuestionPage>(chapterPageReference, CultureInfo.GetCultureInfo(selectedLanguage));
                        questionPage.Name = $"{questionNamePrefix} {question.QuestionNumber}";
                        questionPage.Question = question.Question;
                        questionPage.AnswerAText = question.Options.ContainsKey("A") ? question.Options["A"] : string.Empty;
                        questionPage.AnswerAValue = "A";
                        questionPage.AnswerBText = question.Options.ContainsKey("B") ? question.Options["B"] : string.Empty;
                        questionPage.AnswerBValue = "B";
                        questionPage.AnswerCText = question.Options.ContainsKey("C") ? question.Options["C"] : string.Empty;
                        questionPage.AnswerCValue = "C";
                        questionPage.AnswerDText = question.Options.ContainsKey("D") ? question.Options["D"] : string.Empty;
                        questionPage.AnswerDValue = "D";
                        questionPage.CorrectAnswer = question.CorrectAnswer;

                        _contentRepository.Save(questionPage, SaveAction.Publish, AccessLevel.NoAccess);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
