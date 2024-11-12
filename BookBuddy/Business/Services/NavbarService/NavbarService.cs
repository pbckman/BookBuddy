using System;
using BookBuddy.Models.DataModels;
using BookBuddy.Models.Pages;
using EPiServer.Find.Cms.Statistics;

namespace BookBuddy.Business.Services.NavbarService;

public class NavbarService : INavbarService
{
    public readonly ILogger<NavbarService> _logger;
    public readonly IContentLoader _contentLoader;
    public readonly IUrlResolver _urlResolver;

    public NavbarService(ILogger<NavbarService> logger, IContentLoader contentLoader, IUrlResolver urlResolver)
    {
        _logger = logger;
        _contentLoader = contentLoader;
        _urlResolver = urlResolver;
    }

    public IEnumerable<NavbarLinkModel> GetNavbarProperties(List<PageReference> pageReferences)
    {
        var navbarItems = new List<NavbarLinkModel>();
        try
        {
            foreach (var pageRef in pageReferences)
            {
                var page = _contentLoader.Get<SitePageData>(pageRef);
                if (page != null)
                {
                    // var iconUrl = page.NavbarIcon != null 
                    //         ? _urlResolver.GetUrl(page.NavbarIcon) // Konvertera ContentReference till URL
                    //         : string.Empty;

                    //     var navbarItem = new NavbarItem
                    //     {
                    //         Title = page.NavbarTitle,
                    //         Url = page.LinkURL,
                    //         IconUrl = iconUrl
                    //     };
                    //     navbarItems.Add(navbarItem);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR :  NavbarService.GetNavbarProperties() : {ex.Message}");
        }
        return navbarItems;
    }
}
