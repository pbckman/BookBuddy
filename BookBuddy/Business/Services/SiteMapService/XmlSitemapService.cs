using BookBuddy.Business.Extensions;
using BookBuddy.Models.Pages;

namespace BookBuddy.Business.Services.SiteMapService
{
    public class XmlSitemapService : IXmlSitemapService
    {
        private readonly IContentLoader _contentLoader;

        public XmlSitemapService(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public List<SitePageData> GetPages(XmlSitemap currentPage)
        {
            var startPage = _contentLoader.GetAncestors(currentPage.ContentLink).FirstOrDefault(x => x is StartPage) as PageData;
            var descendants = Enumerable.Empty<SitePageData>();

            if (startPage != null)
            {
                descendants = _contentLoader.GetDescendentsAndSelf(startPage.ContentLink);
            }

            return descendants.ToList();
        }
    }
}
