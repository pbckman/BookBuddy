using BookBuddy.Models.Pages;

namespace BookBuddy.Business.Services.Interfaces
{
    public interface IXmlSitemapService
    {
        List<SitePageData> GetPages(XmlSitemap currentPage);
    }
}
