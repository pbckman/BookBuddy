using BookBuddy.Business.Services.ErrorMessageService;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
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

        [Route("/{culture}/error")]
        public IActionResult Index(string culture, int statusCode, ErrorPage currentPage)
        {
            if (string.IsNullOrEmpty(culture) || (culture != "sv" && culture != "en"))
            {
                culture = "en"; 
            }

            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);

            // Skapa modellen med rätt språk
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
