using static BookBuddy.Globals;

namespace BookBuddy.Models.Pages
{
    [ContentType(
       GUID = "7E32FCE7-7556-44D8-8E32-DAC5E584AB5D",
       DisplayName = "XML Sitemap",
       Description = "This is a XML sitemap template",
       GroupName = GroupNames.Specialized
   )]
    public class XmlSitemap : SitePageData
    {
        [CultureSpecific]
        public virtual ContentReference? SiteSettingsPage { get; set; }
    }
}
