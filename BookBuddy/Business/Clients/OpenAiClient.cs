using Newtonsoft.Json;
using System.Text;

namespace BookBuddy.Business.Clients
{
    public class OpenAiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OpenAiClient> _logger;
        private readonly string _projectKey;
        private readonly string _projectId;

        public OpenAiClient(HttpClient httpClient, ILogger<OpenAiClient> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _projectKey = configuration["OpenAI:ProjectKey"]!;
            _projectId = configuration["OpenAI:ProjectId"]!;


            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_projectKey}");
            _httpClient.DefaultRequestHeaders.Add("OpenAI-Project", _projectId);
        }

        public StringContent CreateRequestContent(string promptMessage, string systemSettings, int maxTokens)
        {
            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
            {
            new { role = "system", content = systemSettings },
            new { role = "user", content = promptMessage }
            },
                temperature = 0.7,
                max_tokens = maxTokens
            };

            return new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        }

        public async Task<string> PostAsync(StringContent content)
        {
            try
            {
                var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<OpenAIResponse>(responseBody);
                    if (responseObject != null) { }
                    var quizContent = responseObject!.Choices[0].Message.Content;

                    return quizContent;
                }

                _logger.LogWarning($"Error while posting to OpenAI: {response.ReasonPhrase}");

            }
            catch (HttpRequestException e)
            {
                _logger.LogError($"Error while posting to OpenAI: {e.Message}");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error while deserializing OpenAI response");
            }

            return null!;
        }
    }

    public class OpenAIResponse
    {
        public Choice[] Choices { get; set; }
    }

    public class Choice
    {
        public Message Message { get; set; }
    }

    public class Message
    {
        public string Content { get; set; }
    }
}
