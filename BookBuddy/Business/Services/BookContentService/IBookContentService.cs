using BookBuddy.Models.DataModels;

namespace BookBuddy.Business.Services.BookContentService
{
    public interface IBookContentService
    {
        Task<List<string>> GetBookChaptersContentAsync(int bookId);
        Task<BookMetadata> GetBookMetadataAsync(int bookId);
        Task<string> GetBookContentAsync(int bookId);
        Task<List<string>> ScrapeBookChaptersContentAsync(int bookId);
    }
}