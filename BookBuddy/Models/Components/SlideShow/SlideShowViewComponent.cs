

//namespace BookBuddy.Models.Components.SlideShow
//{
//    public class SlideShowViewComponent
//    {
//        private readonly IContentLoader _contentLoader;


//        public SlideShowViewComponent(IContentLoader contentLoader)
//        {
//            _contentLoader = contentLoader;

//        }

//        public IViewComponentResult Invoke()
//        {
//            // Hämta boklistan från factory
//            var books = _bookSelectionFactory.GetSelections(null)
//                         .Select(item => new BookViewModel
//                         {
//                             Title = item.Text,
//                             BookId = item.Value

//                         })
//                         .ToList();

//            var model = new BookCarouselViewModel
//            {
//                Books = books
//            };

//            return View("Default", model);
//        }

//        public IViewComponentResult Invoke()
//        {
//            var startPage = _contentLoader.Get<StartPage>(SiteDefinition.Current.StartPage);


//            var model = new SlideShowViewComponentModel
//            {
//                Pages = new List<SlideShowPage>(),
//                BookCards = new List<BookCard>()
//            };

//            if (startPage.SlideShowArea != null)
//            {
//                foreach (var item in startPage.SlideShowArea.FilteredItems)
//                {
//                    //var page = item.GetContent() as SlideShowPage;
//                    //if (page != null)
//                    //{
//                    //    model.Pages.Add(page);

//                    //}

//                    var book = item.GetContent() as BookCard;
//                    if (book != null)
//                    {
//                        model.BookCards.Add(book);
//                    }
//                }
//            }
//            return View("~/Models/Component/SlideShow/SlideShow.razor", model);
//            //return View("~/Models/Components/SlideShow/Default.cshtml", model);
//        }
//    }
//}
