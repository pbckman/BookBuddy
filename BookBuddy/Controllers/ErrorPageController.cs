using BookBuddy.Business.Services;
using BookBuddy.Models.Pages;
using BookBuddy.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index(int statusCode, ErrorPage currentPage)
        {

            var model = new ErrorPageViewModel(currentPage, null!)
            {
                StatusCode = statusCode,
                ErrorMessage = _errorMessageService.GetErrorMessage(statusCode),
                ErrorText = _errorMessageService.GetErrorMessage(0, "errorText"),
                GoToHomeButtonText = _errorMessageService.GetErrorMessage(0, "goToHome")

            };
            return View("Index", model);
        }

    }
}
