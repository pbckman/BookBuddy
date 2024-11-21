using BookBuddy.Business.Services.AccountService;
using BookBuddy.Business.Services.TranslationService;
using BookBuddy.Models.ViewModels;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BookBuddy.Controllers;

[Authorize]
public class AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AccountService accountService, AuthTranslationService translationService) : Controller
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly AccountService _accountService = accountService;
    private readonly AuthTranslationService _translationService = translationService;

    [HttpGet]
    public IActionResult UpdateUser(string? lang)
    {
        var currentCulture = !string.IsNullOrEmpty(lang) ? lang : CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        ViewData["Title"] = _translationService.GetTranslation("updateuser", "title", currentCulture);
        ViewData["Description"] = _translationService.GetTranslation("updateuser", "description", currentCulture);
        ViewData["CurrentPasswordPlaceholder"] = _translationService.GetTranslation("updateuser", "currentPasswordPlaceholder", currentCulture);
        ViewData["CurrentPasswordLabel"] = _translationService.GetTranslation("updateuser", "currentPasswordLabel", currentCulture);
        ViewData["NewPasswordPlaceholder"] = _translationService.GetTranslation("updateuser", "newPasswordPlaceholder", currentCulture);
        ViewData["NewPasswordLabel"] = _translationService.GetTranslation("updateuser", "newPasswordLabel", currentCulture);
        ViewData["ConfirmNewPasswordPlaceholder"] = _translationService.GetTranslation("updateuser", "confirmNewPasswordPlaceholder", currentCulture);
        ViewData["ConfirmNewPasswordLabel"] = _translationService.GetTranslation("updateuser", "confirmNewPasswordLabel", currentCulture);
        ViewData["UpdateButton"] = _translationService.GetTranslation("updateuser", "updateButton", currentCulture);
        ViewData["StatusMessage"] = "";
        ViewData["ErrorMessage"] = "";
        ViewData["AccessTitle"] = _translationService.GetTranslation("updateuser", "accessDenied", currentCulture);
        ViewData["AccessMessage"] = _translationService.GetTranslation("updateuser", "mainProfileNeeded", currentCulture);

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUser(UpdateUserViewModel model, string lang)
    {
        var currentCulture = !string.IsNullOrEmpty(lang) ? lang : CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = _translationService.GetTranslation("updateuser", "errorMessage", currentCulture);
            return RedirectToAction("UpdateUser", "Account");
        }

        var user = await _userManager.GetUserAsync(User);

        var result = await _accountService.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (!result.Succeeded)
        {
            TempData["ErrorMessage"] = _translationService.GetTranslation("updateuser", "errorMessage", currentCulture);
            return RedirectToAction("UpdateUser", "Account");
        }
        TempData["StatusMessage"] = _translationService.GetTranslation("updateuser", "statusMessage", currentCulture);
        return RedirectToAction("UpdateUser", "Account");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var deleteResult = await _userManager.DeleteAsync(user);
        if (!deleteResult.Succeeded)
        {
            ViewData["StatusMessage"] = "Failed to delete account";
            return View(user);
        }

        await _signInManager.SignOutAsync();
        return RedirectToAction("SignIn", "Auth");
    }
}
