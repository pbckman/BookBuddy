using System;
using System.Xml.Linq;

namespace BookBuddy.Business.Services.TranslationService;

public class TranslationService : ITranslationService
{
    private readonly string _translationsPath;
    private readonly ILogger<TranslationService> _logger;

    public TranslationService(string translationsPath, ILogger<TranslationService> logger)
    {
        _translationsPath = translationsPath;
        _logger = logger;
    }

    public string GetTranslation(string fileName, string component, string key, string languageId)
    {
        try
        {
            var filePath = System.IO.Path.Combine(_translationsPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                _logger.LogError($"ERROR : TranslationService.GetTranslation() : File {fileName} not found at {filePath}");
                return $"[{component}.{key}]";
            }

            var translations = XElement.Load(filePath);
            var translation = translations.Descendants("language")
                .FirstOrDefault(lang => lang.Attribute("id")?.Value == languageId)?
                .Descendants(component)
                .FirstOrDefault()?
                .Element("properties")?
                .Element(key)?.Value;

            return translation ?? $"[{component}.{key}]";
            
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : TranslationService.GetTranslation() : {ex.Message}");
        }
            return $"[{component}.{key}]";
    }

    public XElement GetComponentElement(string fileName, string component, string languageId)
    {
        try
        {
            var filePath = System.IO.Path.Combine(_translationsPath, fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return null!;
            }

            var translations = XElement.Load(filePath);
            var componentElement = translations.Descendants("language")
                .FirstOrDefault(lang => lang.Attribute("id")?.Value == languageId)?
                .Descendants(component)
                .FirstOrDefault();

            return componentElement ?? new XElement(component);
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : TranslationService.GetComponentElement() : {ex.Message}");
        }
        return new XElement(component);
    }
}
