using System;
using System.Xml.Linq;
using BookBuddy.Business.Services.TranslationService;

namespace BookBuddy.Business.Factories;

public class TranslationFactory
{
    private readonly ITranslationService _translationService;

        public TranslationFactory(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        public Dictionary<string, string> GetTranslationsForView(string fileName, string component, string lang)
        {
            var translations = new Dictionary<string, string>();

            XElement componentElement = _translationService.GetComponentElement(fileName, component, lang);

            if (componentElement != null)
            {
                var propertiesElement = componentElement.Element("properties");
                if (propertiesElement != null)
                {
                    foreach (var element in propertiesElement.Elements())
                    {
                        string key = element.Name.LocalName;
                        string value = element.Value;
                        translations[key] = value;
                    }
                }
            }

            return translations;
        }
}
