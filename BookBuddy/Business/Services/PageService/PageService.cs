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

    public List<BookPage> GetBookPages(string currentCulture)
    {
        var languageContainerPage = GetLanguageContainerPage(currentCulture);
        if (languageContainerPage == null)
        {
            return null!;
        }

        var bookPages = _contentLoader.GetChildren<BookPage>(languageContainerPage.ContentLink, new LanguageSelector(currentCulture)).ToList();
        return bookPages ?? [];
    }

    public AvailableBooksPage GetAvailableBooksPage(string currentCulture)
    {
        var bookContainerPage = GetBookContainerPage(currentCulture);
        if (bookContainerPage == null)
        {
            return null!;
        }

        var availableBookPage = _contentRepository.GetChildren<AvailableBooksPage>(bookContainerPage.ContentLink, new LanguageSelector(currentCulture)).FirstOrDefault();

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
}
