using BookBuddy.Business.Clients;
using BookBuddy.Business.Extensions;
using BookBuddy.Models.DataModels;
using Newtonsoft.Json;

namespace BookBuddy.Business.Services.AiService
{
    public class OpenAiService : IAiService
    {
        private readonly OpenAiClient _openAiClient;
        private readonly ILogger<OpenAiService> _logger;

        public OpenAiService(OpenAiClient openAiClient, ILogger<OpenAiService> logger)
        {
            _openAiClient = openAiClient;
            _logger = logger;
        }

        public async Task<QuizChapterModel> GenerateQuizQuestionsAsync(string chapterContent, string bookTitle, string lang)
        {
            try
            {
                var content = _openAiClient.CreateRequestContent(chapterContent,
                    $"The response must be in {lang} and in json-format and be deserializable to this model: " +
                    "public class QuizChapterModel{[JsonProperty(\"chapter_title\")]public string ChapterTitle { get; set; }[JsonProperty(\"questions\")]public List<QuizQuestionModel> Questions { get; set; } = [];}public class QuizQuestionModel{[JsonProperty(\"question_number\")]public int QuestionNumber { get; set; }[JsonProperty(\"question\")]public string Question { get; set; }[JsonProperty(\"options\")]public Dictionary<string, string> Options { get; set; }[JsonProperty(\"correct_answer\")]public string CorrectAnswer { get; set; }}" +
                    $"You are an assistant that generates 5 multiple-choice (4 choices per question) questions to improve reading comprehension based on the text of a given chapter of the book {bookTitle}" +
                    "IMPORTANT: chapter_title include the whole chapter title from the book, including prefix and number if there is any" +
                    $"IMPORTANT: The whole response must be in {lang}!!" +
                    $"IMPORTANT: The chapter_title must be in {lang}!!"
                   ,
                    3000);

                var result = await _openAiClient.PostAsync(content);

                result = result.ExtractJson();

                if (result != null)
                {
                    var chapterQuiz = JsonConvert.DeserializeObject<QuizChapterModel>(result)!;
                    return chapterQuiz ?? null!;
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error while deserializing quiz questions");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while generating quiz questions");
            }

            return null!;
        }

        public async Task<MetadataDetails> GenerateMetadataAsync(string bookContent, string lang)
        {
            try
            {
                if(bookContent.Length >250000)
                {
                    bookContent = bookContent.Substring(0, 250000);
                }
               
                var content = _openAiClient.CreateRequestContent(bookContent,
                    $"The response must be in {lang} and in JSON format and it should be deserializable to this model: " +
                    "public class MetadataDetails{public string Title { get; set; }public string Plot { get; set; }}" +
                    "IMPORTANT: Include the quotation marks if there is any!" +
                    "IMPORTANT: Plot should contain approximately 700 characters." +
                    $"IMPORTANT: Both title and plot must be in {lang}!"
                    , 10000);

                var result = await _openAiClient.PostAsync(content);

                result = result.ExtractJson();

                if (result == null)
                    return null!;

                var bookDataModel = JsonConvert.DeserializeObject<MetadataDetails>(result)!;
                if (bookDataModel != null)
                    return bookDataModel;

            }
            catch (Exception)
            {
            }

            return null!;
        }
    }
}
