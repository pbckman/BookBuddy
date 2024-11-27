using System.Globalization;
using System.Xml.Linq;

namespace BookBuddy.Business.Services.ErrorMessageService
{
    public class ErrorMessageService
    {
        private readonly string _filePath;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ErrorMessageService(IHttpContextAccessor httpContextAccessor)
        {
            _filePath = "Resources/Translations/Error.xml";
            _httpContextAccessor = httpContextAccessor;
        }


        public string GetErrorMessage(int errorCode, string key = null!, string culture = null!)
        {
            // Om ingen kultur specificeras, hämta den aktuella kulturen från tråden
            culture ??= CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            var doc = XDocument.Load(_filePath);
            var languageNode = doc.Descendants("language")
                                  .FirstOrDefault(l => l.Attribute("id")?.Value == culture);

            // Om det efterfrågade språket inte finns, använd engelska som fallback
            if (languageNode == null)
            {
                culture = "en";
                languageNode = doc.Descendants("language")
                                  .FirstOrDefault(l => l.Attribute("id")?.Value == culture);
            }

            // Om ett specifikt errorCode finns, hämta felmeddelandet
            if (errorCode > 0)
            {
                var message = languageNode?.Descendants("error")
                                          .Where(e => (int)e.Attribute("code") == errorCode)
                                          .Elements("caption")
                                          .FirstOrDefault()?.Value;

                if (!string.IsNullOrEmpty(message))
                    return message;
            }

            // Om inget errorCode eller specifik text efterfrågas, hämta från <common>
            if (!string.IsNullOrEmpty(key))
            {
                var commonText = languageNode?.Descendants("common")
                                            .Elements(key)
                                            .FirstOrDefault()?.Value;

                return commonText ?? $"[{key}] text not found";
            }

            return "An error has occurred.";
        }


    }
}
