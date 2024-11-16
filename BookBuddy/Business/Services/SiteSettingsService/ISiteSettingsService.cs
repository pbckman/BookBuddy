using System;
using BookBuddy.Models.Pages;

namespace BookBuddy.Business.Services.SiteSettingsService;

public interface ISiteSettingsService
{
    SiteSettingsPage GetSiteSettings(ContentReference siteSettingsReference);
}
