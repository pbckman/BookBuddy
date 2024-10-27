using BookBuddy.Business.Services.Interfaces;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace BookBuddy.Controllers
{
    public class XmlSitemapController : PageControllerBase<XmlSitemap>
    {
        private readonly IContentLoader _contentLoader;
        private readonly IXmlSitemapService _xmlSitemapService;

        public XmlSitemapController(IContentLoader contentLoader, IXmlSitemapService xmlSitemapService)
        {
            _contentLoader = contentLoader;
            _xmlSitemapService = xmlSitemapService;
        }

        public IActionResult Index(XmlSitemap currentPage)
        {
            var siteSettingsReference = currentPage.SiteSettingsPage;

            SiteSettingsPage siteSettings = null;
            if (siteSettingsReference != null)
            {
                siteSettings = _contentLoader.Get<SiteSettingsPage>(siteSettingsReference);
            }
            var model = new XmlSitemapViewModel(currentPage, siteSettings)
            {
                Pages = _xmlSitemapService.GetPages(currentPage)
            };

            return View(model);
        }
    }
}
