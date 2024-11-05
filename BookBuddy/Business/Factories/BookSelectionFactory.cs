using BookBuddy.Business.Services.BookService;
using EPiServer.Globalization;
using EPiServer.Shell.ObjectEditing;

namespace BookBuddy.Business.Factories;

public class BookSelectionFactory : ISelectionFactory
{
    private IBookService _bookService;

    public BookSelectionFactory(IBookService bookService)
    {
        _bookService = bookService;
    }

    public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
    {

        var currentCulture = ContentLanguage.PreferredCulture.Name;
        var allBooks = _bookService.GetAllBooks(currentCulture);
        var currentBooksId = _bookService.GetCurrentBooks(currentCulture);

        if (allBooks == null || currentBooksId == null)
            return new List<SelectItem>();

        var availableBooks = allBooks.Where(book => !currentBooksId.Any(id => id == book.BookId)).ToList();

        return availableBooks.Select(book => new SelectItem
        {
            Text = book.Title,
            Value = book.BookId
        });
    }
}
