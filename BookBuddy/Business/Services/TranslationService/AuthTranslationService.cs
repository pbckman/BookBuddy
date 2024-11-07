using System.Xml.Linq;

namespace BookBuddy.Business.Services.TranslationService
{
    public class AuthTranslationService
    {
        private readonly XElement _translations;

        public AuthTranslationService(string filePath)
        {
            _translations = XElement.Load(filePath);
        }

        public string GetTranslation(string component, string key, string languageId)
        {
            var translation = _translations.Descendants("language")
                .FirstOrDefault(lang => lang.Attribute("id")?.Value == languageId)?
                .Descendants(component)
                .FirstOrDefault()?
                .Element("properties")?
                .Element(key)?.Value;

            return translation ?? key;
        }
    }
}
