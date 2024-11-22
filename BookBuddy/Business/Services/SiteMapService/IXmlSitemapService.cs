using BookBuddy.Models.Pages;

namespace BookBuddy.Business.Services.SiteMapService
{
    public interface IXmlSitemapService
    {
        List<SitePageData> GetPages(XmlSitemap currentPage);
    }
}
