using System.Globalization;
using System.Xml.Linq;

namespace BookBuddy.Business.Services
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

        public string GetErrorMessage(int errorCode, string key = null)
        {
            var culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            var doc = XDocument.Load(_filePath);

            // Om ett specifikt errorCode finns, hämta felmeddelandet
            if (errorCode > 0)
            {
                var message = doc.Descendants("language")
                                 .Where(l => l.Attribute("id")?.Value == culture)
                                 .Descendants("error")
                                 .Where(e => (int)e.Attribute("code") == errorCode)
                                 .Elements("caption")
                                 .FirstOrDefault()?.Value;

                if (!string.IsNullOrEmpty(message))
                    return message;
            }

            // Om inget errorCode eller specifik text efterfrågas, hämta från <common>
            if (!string.IsNullOrEmpty(key))
            {
                var commonText = doc.Descendants("language")
                                    .Where(l => l.Attribute("id")?.Value == culture)
                                    .Descendants("common")
                                    .Elements(key)
                                    .FirstOrDefault()?.Value;

                return commonText ?? $"[{key}] text not found";
            }

            return "An error has occurred."; 
        }

    }
}
