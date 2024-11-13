﻿using BookBuddy.Business.Services.AccountService;
using BookBuddy.Business.Services.TranslationService;
using BookBuddy.Models.ViewModels;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers;

[Authorize]
public class AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AccountService accountService, AuthTranslationService translationService) : Controller
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly AccountService _accountService = accountService;
    private readonly AuthTranslationService _translationService = translationService;


    [HttpGet]
    [Route("{lang}/account/updateuser")]
    public IActionResult UpdateUser(string lang = "en")
    {
        ViewData["Title"] = _translationService.GetTranslation("updateuser", "title", lang);
        ViewData["Description"] = _translationService.GetTranslation("updateuser", "description", lang);
        ViewData["CurrentPasswordPlaceholder"] = _translationService.GetTranslation("updateuser", "currentPasswordPlaceholder", lang);
        ViewData["NewPasswordPlaceholder"] = _translationService.GetTranslation("updateuser", "newPasswordPlaceholder", lang);
        ViewData["ConfirmNewPasswordPlaceholder"] = _translationService.GetTranslation("updateuser", "confirmNewPasswordPlaceholder", lang);
        ViewData["UpdateButton"] = _translationService.GetTranslation("updateuser", "updateButton", lang);
        ViewData["StatusMessage"] = "";
        ViewData["AccessTitle"] = _translationService.GetTranslation("updateuser", "accessDenied", lang);
        ViewData["AccessMessage"] = _translationService.GetTranslation("updateuser", "mainProfileNeeded", lang);

        return View();
    }

    [HttpPost]
    [Route("{lang}/account/updateuser")]
    public async Task<IActionResult> UpdateUser(UpdateUserViewModel model, string lang = "en")
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return Unauthorized();
        }

        var result = await _accountService.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (!result.Succeeded)
        {
            ViewData["StatusMessage"] = _translationService.GetTranslation("updateuser", "errorMessage", lang);
            return View(model);
        }

        return RedirectToAction("SignIn", "Auth");
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