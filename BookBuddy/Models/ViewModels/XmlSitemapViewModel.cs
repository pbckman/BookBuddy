using BookBuddy.Models.Pages;

namespace BookBuddy.Models.ViewModels
{
    public class XmlSitemapViewModel(XmlSitemap currentPage, SiteSettingsPage siteSettings) : PageViewModel<XmlSitemap>(currentPage, siteSettings)
    {
        public List<SitePageData> Pages { get; set; }
    }
}
