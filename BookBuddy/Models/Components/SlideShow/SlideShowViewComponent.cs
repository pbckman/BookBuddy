using BookBuddy.Models.Pages;
using EPiServer.Web;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Models.Components.SlideShow
{
    public class SlideShowViewComponent : ViewComponent
    {
        private readonly IContentLoader _contentLoader;

        public SlideShowViewComponent(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public IViewComponentResult Invoke()
        {
            var startPage = _contentLoader.Get<StartPage>(SiteDefinition.Current.StartPage);

            var model = new SlideShowViewComponentModel
            {
                Pages = new List<SlideShowPage>()
            };
           
            if(startPage.SlideShowArea != null)
            {
                foreach(var item in startPage.SlideShowArea.FilteredItems)
                {
                    var page = item.GetContent() as SlideShowPage;
                    if(page != null)
                    {
                        model.Pages.Add(page);
                    }
                }
            }

            return View("~/Models/Components/SlideShow/Default.cshtml", model);
        }
    }
}
