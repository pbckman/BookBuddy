using System;
using BookBuddy.Models.Pages;

namespace BookBuddy.Business.Services.SiteSettingsService;

public class SiteSettingsService : ISiteSettingsService
{
    private readonly IContentLoader _contentLoader;

    public SiteSettingsService(IContentLoader contentLoader)
    {
        _contentLoader = contentLoader;
    }

    public SiteSettingsPage? GetSiteSettings(ContentReference? siteSettingsReference)
    {
        return siteSettingsReference != null ? _contentLoader.Get<SiteSettingsPage>(siteSettingsReference) : null;
    }
}
