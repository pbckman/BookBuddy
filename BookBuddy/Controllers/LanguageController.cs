using System;
using System.Globalization;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers;

public class LanguageController(IUrlResolver urlResolver, IContentLoader contentLoader, ILogger<LanguageController> logger) : Controller
{
    private readonly IUrlResolver _urlResolver = urlResolver;
    private readonly IContentLoader _contentLoader = contentLoader;
    private readonly ILogger<LanguageController> _logger = logger;

    public IActionResult ChangeLanguage(ContentReference contentReference, string language)
    {
        try
        {
            if (_contentLoader.TryGet(contentReference, new CultureInfo(language), out IContent content))
            {
                string localizedUrl = _urlResolver.GetUrl(contentReference, language);

                if (!string.IsNullOrEmpty(localizedUrl))
                {
                    return Redirect(localizedUrl);
                }
            }

            string fallbackUrl = _urlResolver.GetUrl(contentReference, Request.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name);

            if (!string.IsNullOrEmpty(fallbackUrl))
            {
                _logger.LogWarning("Could not find content in language {Language}. Redirecting to fallback URL {FallbackUrl}", language, fallbackUrl);
                return Redirect(fallbackUrl);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while changing language");
        }
        return RedirectToAction("Index", "StartPage");
    }
}
