using BookBuddy.Business.Services.ErrorMessageService;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using EPiServer.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BookBuddy.Controllers
{
    [AllowAnonymous]
    public class ErrorPageController : PageControllerBase<ErrorPage>
    {
        private readonly ErrorMessageService _errorMessageService;
        public ErrorPageController(ErrorMessageService errorMessageService)
        {
            _errorMessageService = errorMessageService;
        }

        [Route("/error")]
        [Route("/{culture}/error")]
        public IActionResult Index(string culture, int statusCode, ErrorPage currentPage)
        {
            culture ??= "en";

            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);

            if (currentPage == null)
            {
                var contentLoader = HttpContext.RequestServices.GetService<IContentLoader>();

                if (culture == "sv")
                {
                    currentPage = contentLoader.GetChildren<ErrorPage>(ContentReference.StartPage)
                                               .FirstOrDefault(p => p.Language.Name == "sv");
                }
                else
                {
                    currentPage = contentLoader.GetChildren<ErrorPage>(ContentReference.StartPage)
                                               .FirstOrDefault(p => p.Language.Name == "en");
                }

                if (currentPage == null)
                {
                    Console.WriteLine($"ErrorPage not found for culture: {culture}");
                    return View("StaticError", new { StatusCode = statusCode });
                }
            }

            var model = new ErrorPageViewModel(currentPage, null!)
            {
                StatusCode = statusCode,
                ErrorMessage = _errorMessageService.GetErrorMessage(statusCode, culture: culture),
                ErrorText = _errorMessageService.GetErrorMessage(0, "errorText", culture),
                GoToHomeButtonText = _errorMessageService.GetErrorMessage(0, "goToHome", culture)
            };

            return View("Index", model);

        }

    }
}
