using BookBuddy.Business.Initialization;
using BookBuddy.Business.Services.BookPageService;
using BookBuddy.Business.Services.BookService;
using BookBuddy.Business.Services.PageService;
using BookBuddy.Business.Services.ScheduledJobsService;
using BookBuddy.Models.Pages;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.Web;

namespace BookBuddy.Business.ScheduledJobs
{
    [ScheduledPlugIn(
    DisplayName = "Generate Book Quiz",
    Description = "This job generates a book quiz",
    GUID = "C5F818B6-B864-4E5C-837B-49BCB98D14EE"
)]
    public class HandleGenerateBook : ScheduledJobBase
    {

        private readonly IContentLoader _contentLoader;
        private readonly ISiteDefinitionRepository _siteDefinitionRepository;
        private readonly IContentRepository _contentRepository;
        private readonly IBookService _bookService;
        private readonly IBookPageService _bookPageService;
        private bool _stopSignaled;
        private readonly IPageService _pageService;
        private readonly IScheduledJobsService _scheduledJobsService;


        public HandleGenerateBook(IContentLoader contentLoader,
            ISiteDefinitionRepository siteDefinitionRepository, IContentRepository contentRepository, IBookService bookService, IBookPageService bookPageService, IPageService pageService, IScheduledJobsService scheduledJobsService)
        {
            _contentLoader = contentLoader;
            _siteDefinitionRepository = siteDefinitionRepository;
            _contentRepository = contentRepository;
            _bookService = bookService;
            _bookPageService = bookPageService;
            IsStoppable = true;
            _pageService = pageService;
            _scheduledJobsService = scheduledJobsService;
        }
        public override string Execute()
        {
            string GetTimestampedMessage(string message) => $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

            OnStatusChanged(GetTimestampedMessage("Job started: Generating book quiz..."));

            var cultureName = PublishBookEventHandler.CurrentCultureName;
            OnStatusChanged(GetTimestampedMessage($"Current culture set to {cultureName}."));

            var availableBooksPage = _pageService.GetAvailableBooksPage(cultureName);
            if (availableBooksPage == null)
                return "AvailableBooksPage could not be found";
            else
                OnStatusChanged(GetTimestampedMessage($"AvailableBooksPage found: {availableBooksPage.Name}"));

            if (_stopSignaled)
                return "Job was stopped";

            var bookQuizModel = _bookService.GetBookQuizAsync(Convert.ToInt32(availableBooksPage.SelectedBook), cultureName).Result;
            if (bookQuizModel == null)
                return "Failed to generate book quiz";
            else
                OnStatusChanged(GetTimestampedMessage($"Book quiz generated: {bookQuizModel.Metadata.Title}"));

            if (_stopSignaled)
                return "Job was stopped";


            bool isCreated = _bookPageService.CreatePages(bookQuizModel, cultureName);
            if (!isCreated)
                return "Failed to create book quiz pages";
            else
                OnStatusChanged(GetTimestampedMessage($"Book quiz pages created: {bookQuizModel.Metadata.Title}"));

            _scheduledJobsService.TriggerIndexing();

            return $"The book {bookQuizModel.Metadata.Title} was generated";
        }
        public override void Stop()
        {
            _stopSignaled = true;
        }
    }
}
