using System;
using System.Xml.Linq;

namespace BookBuddy.Business.Services.TranslationService;

public interface ITranslationService
{
    string GetTranslation(string fileName, string component, string key, string languageId);
    XElement GetComponentElement(string fileName, string component, string languageId);
}
