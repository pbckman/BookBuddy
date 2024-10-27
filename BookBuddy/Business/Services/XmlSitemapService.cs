using BookBuddy.Business.Extensions;
using BookBuddy.Business.Services.Interfaces;
using BookBuddy.Models.Pages;
using MimeKit.Cryptography;

namespace BookBuddy.Business.Services
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
