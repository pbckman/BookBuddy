using BookBuddy.Models.Pages;

namespace BookBuddy.Business.Services.XmlSitemapService
{
    public interface IXmlSitemapService
    {
        List<SitePageData> GetPages(XmlSitemap currentPage);
    }
}
