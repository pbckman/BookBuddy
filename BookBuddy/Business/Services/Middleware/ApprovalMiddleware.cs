using BookBuddy.Models.Pages;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Find.Cms.Statistics;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BookBuddy.Business.Services.Middleware
{
    public class ApprovalMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;

        public ApprovalMiddleware(RequestDelegate next, UserManager<ApplicationUser> userManager, IContentLoader contentLoader, UrlResolver urlResolver)
        {
            _next = next;
            _userManager = userManager;
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated && !context.User.HasClaim("AlreadyRedirected", "true"))
            {
                var user = await _userManager.GetUserAsync(context.User);
                if (user != null && !user.IsApproved)
                {
                    // Lägg till en claim eller annan flagga för att förhindra framtida omdirigeringar.
                    var identity = (ClaimsIdentity)context.User.Identity;
                    identity.AddClaim(new Claim("AlreadyRedirected", "true"));

                    var booksPage = _contentLoader.GetChildren<BooksPage>(ContentReference.StartPage).FirstOrDefault();
                    if (booksPage != null)
                    {
                        var booksPageUrl = _urlResolver.GetUrl(booksPage.ContentLink);
                        context.Response.Redirect(booksPageUrl);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("BooksPage not found.");
                    }
                }
            }

            await _next(context);
        }

    }
}
