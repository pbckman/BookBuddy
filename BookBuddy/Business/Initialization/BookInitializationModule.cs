using EPiServer.Data.Dynamic;
using EPiServer.Framework.Initialization;
using EPiServer.Framework;
using BookBuddy.Business.Helpers;
using BookBuddy.Models.DDS;

namespace BookBuddy.Business.Initialization
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class BookInitializationModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var store = DynamicDataStoreFactory.Instance.GetStore(typeof(Book)) ?? DynamicDataStoreFactory.Instance.CreateStore(typeof(Book));


            if (store.Items<Book>().Count() == 0)
            {

                var books = new List<Book>();
                books.AddRange(InitialBookData.BookDropdownItemsEN);
                books.AddRange(InitialBookData.BookDropdownItemsSV);
                foreach (var book in books)
                {
                    store.Save(book);
                }
            }
        }

        public void Uninitialize(InitializationEngine context)
        {
            
        }
    }
}
