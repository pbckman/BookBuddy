using BookBuddy.Models.Pages;
using EPiServer.Web.Mvc;

namespace BookBuddy.Controllers
{
    public abstract class PageControllerBase<T> : PageController<T> where T : SitePageData
    {
    }
}
