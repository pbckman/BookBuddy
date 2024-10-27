using BookBuddy.Models.Pages;

namespace BookBuddy.Business.Extensions
{
    public static class ContentLoaderExtensions
    {
        public static IEnumerable<SitePageData> GetDescendentsAndSelf(this IContentLoader contentLoader, ContentReference startPageReference) 
        {
            var startPage = contentLoader.Get<SitePageData>(startPageReference);

            var descendants = contentLoader.GetDescendents(startPageReference)
                .Select(contentLoader.Get<IContent>)
                .Where(content => content is SitePageData && content is not XmlSitemap)
                .Cast<SitePageData>();

            return new[] {startPage}.Concat(descendants);
        }
    }
}
