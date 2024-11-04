using BookBuddy.Models.DataModels;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace BookBuddy.Business.Services.BookContentService
{
    public class GutenbergService : IBookContentService
    {
        private readonly HttpClient _client;
        private readonly ILogger<IBookContentService> _logger;

        public GutenbergService(HttpClient client, ILogger<IBookContentService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<List<string>> GetBookChaptersContentAsync(int bookId)
        {
            try
            {
                string url = $"https://www.gutenberg.org/files/{bookId}/{bookId}-h/{bookId}-h.htm";
                var response = await _client.GetAsync(url);


                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error while getting book content");
                }
                var html = await response.Content.ReadAsStringAsync();

                var document = new HtmlDocument();
                document.LoadHtml(html);

                var chapters = document.DocumentNode.SelectNodes("//div[contains(@class, 'chapter')]");

                if (chapters != null && chapters.Any())
                {
                    return chapters.
                        Where(chapter =>
                                chapter.SelectSingleNode(".//h2") != null &&
                                chapter.SelectSingleNode(".//h2").HasChildNodes &&
                                chapter.SelectSingleNode(".//h2").ChildNodes.Any(x => x.Name == "a") &&
                                chapter.SelectSingleNode(".//h2").SelectSingleNode(".//a").Attributes["name"] != null &&
                                chapter.SelectSingleNode(".//h2").SelectSingleNode(".//a").Attributes["name"].Value != "chap00" &&
                                !chapter.SelectSingleNode(".//h2").SelectSingleNode(".//a").Attributes["name"].Value.Contains("pref") &&
                                chapter.InnerText.Length > 500 &&
                                chapter.SelectNodes(".//p") != null).Select(x => x.InnerText).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting book content");
            }

            return null!;

        }
        public async Task<List<string>> ScrapeBookChaptersContentAsync(int bookId)
        {
            try
            {
                string url = $"https://www.gutenberg.org/files/{bookId}/{bookId}-h/{bookId}-h.htm";

                var html = await _client.GetStringAsync(url);

                var document = new HtmlDocument();

                document.LoadHtml(html);

                var chapters = document.DocumentNode.SelectNodes("//div[contains(@class, 'chapter')]");

                if (chapters != null)
                {
                    foreach (var chapter in chapters)
                    {

                    }
                }



                _logger.LogError("Error while getting book content");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting book content");
            }

            return null!;

        }

        public async Task<string> GetBookContentAsync(int bookId)
        {
            try
            {
                var response = await _client.GetAsync($"https://www.gutenberg.org/files/{bookId}/{bookId}-0.txt");
                if (response.IsSuccessStatusCode)
                {

                    return await response.Content.ReadAsStringAsync();
                }


                _logger.LogError("Error while getting book content");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting book content");
            }

            return null!;
        }

        public async Task<BookMetadata> GetBookMetadataAsync(int bookId)
        {
            try
            {
                var response = await _client.GetAsync($"https://gutendex.com/books/{bookId}/");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var bookMetadata = JsonConvert.DeserializeObject<BookMetadata>(jsonString);
                    return bookMetadata ?? null!;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting book metadata");
            }

            return null!;
        }

    }
}
