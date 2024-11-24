using BookBuddy.Models.Pages;
using BookBuddy.Models.Pages.Containers;
using EPiServer.Web;

namespace BookBuddy.Business.Services.PageService;

public class PageService : IPageService
{
    private readonly IContentLoader _contentLoader;
    private readonly ISiteDefinitionRepository _siteDefinitionRepository;
    private readonly IContentRepository _contentRepository;

    public PageService(ISiteDefinitionRepository siteDefinitionRepository, IContentLoader contentLoader, IContentRepository contentRepository)
    {
        _siteDefinitionRepository = siteDefinitionRepository;
        _contentLoader = contentLoader;
        _contentRepository = contentRepository;
    }

    public StartPage GetStartPage(string currentCulture)
    {
        var siteDefinition = _siteDefinitionRepository.List().FirstOrDefault();
        if (siteDefinition == null || ContentReference.IsNullOrEmpty(siteDefinition.StartPage))
        {
            throw new InvalidOperationException("StartPage reference is not set.");
        }

        var startPage = _contentLoader.Get<StartPage>(siteDefinition.StartPage, new LanguageSelector(currentCulture));
        return startPage ?? null!;
    }

    public LanguageContainerPage GetLanguageContainerPage(string currentCulture)
    {
        try
        {
            var bookContainerPage = GetBookContainerPage(currentCulture);
            if (bookContainerPage == null)
            {
                return null!;
            }

            var languageContainerPage = _contentRepository.GetChildren<LanguageContainerPage>(bookContainerPage.ContentLink, new LanguageSelector(currentCulture)).FirstOrDefault();

            return languageContainerPage ?? null!;
        }
        catch (Exception)
        {
        }

        return null!;
    }

    public BooksPage GetBooksPage(string currentCulture)
    {
        try
        {
            var startPage = GetStartPage(currentCulture);
            if (startPage == null)
            {
                return null!;
            }

            var booksPage = _contentRepository.GetChildren<BooksPage>(startPage.ContentLink, new LanguageSelector(currentCulture)).FirstOrDefault();

            return booksPage ?? null!;
        }
        catch (Exception)
        {
        }

        return null!;
    }

    public List<BookPage> GetBookPages(string currentCulture)
    {
        var booksPage = GetBooksPage(currentCulture);
        if (booksPage == null)
        {
            return null!;
        }

        var bookPages = _contentLoader.GetChildren<BookPage>(booksPage.ContentLink, new LanguageSelector(currentCulture)).ToList();
        return bookPages ?? [];
    }

    public AvailableBooksPage GetAvailableBooksPage(string currentCulture)
    {
        var booksPage = GetBooksPage(currentCulture);
        if (booksPage == null)
        {
            return null!;
        }

        var availableBookPage = _contentRepository.GetChildren<AvailableBooksPage>(booksPage.ContentLink, new LanguageSelector(currentCulture)).FirstOrDefault();

        return availableBookPage ?? null!;

    }

    private BookContainerPage GetBookContainerPage(string currentCulture)
    {
        var startPage = GetStartPage(currentCulture);
        if (startPage == null)
        {
            return null!;
        }
        var bookContainerPage = _contentLoader.GetChildren<BookContainerPage>(startPage.ContentLink, new LanguageSelector(currentCulture)).FirstOrDefault();

        return bookContainerPage ?? null!;
    }

    public QuizPage GetQuizPageById(int quizPageId, string lang)
    {
        var quizPage = _contentLoader.Get<QuizPage>(new ContentReference(quizPageId), new LanguageSelector(lang));
        return quizPage ?? null!;
    }

    public MyQuizzesPage GetMyQuizzesPage(string lang)
    {
        try
        {
            var startPage = GetStartPage(lang);
            var myQuizzesPage = _contentLoader.GetChildren<MyQuizzesPage>(startPage.ContentLink, new LanguageSelector(lang)).FirstOrDefault();
            return myQuizzesPage ?? null!;
        }
        catch (Exception ex)
        {
        }

        return null!;
    
    }

    public AchievementsPage GetAchievementsPage(string lang)
    {
        var booksPage = GetBooksPage(lang);
        if (booksPage == null)
        {
            return null!;
        }
        var achievementsPage = _contentRepository.GetChildren<AchievementsPage>(booksPage.ContentLink, new LanguageSelector(lang)).FirstOrDefault();

        return achievementsPage ?? null!;
    }

    public List<QuizPage> GetQuizPages(string lang)
    {
        try
        {
            var booksPage = GetBooksPage(lang);
            if (booksPage == null)
            {
                return null!;
            }
            var bookPages = _contentLoader.GetChildren<BookPage>(booksPage.ContentLink, new LanguageSelector(lang)).ToList();
            if (bookPages == null)
            {
                return null!;
            }
            var quizPages = new List<QuizPage>();
            foreach (var bookPage in bookPages)
            {
                var quizPage = _contentRepository.GetChildren<QuizPage>(bookPage.ContentLink, new LanguageSelector(lang)).FirstOrDefault();
                if (quizPage != null)
                {
                    quizPages.Add(quizPage);
                }
            }

            return quizPages;
        }
        catch (Exception)
        {
        }

        return null!;

    }
}
