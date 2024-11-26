using BookBuddy.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookBuddy.Controllers
{
    public class ProfileController(UserManager<ApplicationUser> userManager, ProfileService profileService, AuthTranslationService translationService) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ProfileService _profileService = profileService;
        private readonly AuthTranslationService _translationService = translationService;

        [HttpPost]
        public async Task<IActionResult> CreateProfile(ProfileViewModel model)
        {
            var currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (model == null || string.IsNullOrEmpty(model.FirstName))
            {
                TempData["ErrorMessageCreate"] = _translationService.GetTranslation("updateprofile", "errorMessageCreate", currentCulture);
                return RedirectToAction("UpdateProfile", "Profile", new { currentCulture });
            }

            await _profileService.CreateSubProfileAsync(user.Id, model.FirstName, model.LastName, isMainProfile: false);

            TempData["StatusMessageCreate"] = _translationService.GetTranslation("updateprofile", "statusMessageCreate", currentCulture);
            return RedirectToAction("UpdateProfile", "Profile", new { currentCulture });
        }

        [HttpPost]
        public IActionResult SelectProfile(int profileId)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7),
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            };

            Response.Cookies.Append("SelectedProfileId", profileId.ToString(), cookieOptions);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult SelectSubProfile(int profileId)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7),
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            };

            Response.Cookies.Append("SelectedSubProfileId", profileId.ToString(), cookieOptions);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile(string lang)
        {
            var currentCulture = !string.IsNullOrEmpty(lang) ? lang : CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            ViewData["Title"] = _translationService.GetTranslation("updateprofile", "title", currentCulture);
            ViewData["Description"] = _translationService.GetTranslation("updateprofile", "description", currentCulture);
            ViewData["FirstNameCreate"] = _translationService.GetTranslation("updateprofile", "firstNameCreate", currentCulture);
            ViewData["SaveButtonCreate"] = _translationService.GetTranslation("updateprofile", "saveButtonCreate", currentCulture);
            ViewData["TitleUpdate"] = _translationService.GetTranslation("updateprofile", "titleUpdate", currentCulture);
            ViewData["DescriptionUpdate"] = _translationService.GetTranslation("updateprofile", "descriptionUpdate", currentCulture);
            ViewData["FirstNameUpdate"] = _translationService.GetTranslation("updateprofile", "firstNameUpdate", currentCulture);
            ViewData["SaveButtonUpdate"] = _translationService.GetTranslation("updateprofile", "saveButtonUpdate", currentCulture);
            ViewData["SelectProfile"] = _translationService.GetTranslation("updateprofile", "selectProfile", currentCulture);
            ViewData["SelectedProfile"] = _translationService.GetTranslation("updateprofile", "selectedProfile", currentCulture);
            ViewData["FirstNamePlaceholder"] = _translationService.GetTranslation("updateprofile", "firstNamePlaceholder", currentCulture);
            ViewData["ErrorMessageUpdate"] = "";
            ViewData["ErrorMessageCreate"] = "";
            ViewData["StatusMessageUpdate"] = "";
            ViewData["StatusMessageCreate"] = "";
            ViewData["StatusMessageUpdateSub"] = "";
            ViewData["ErrorMessageUpdateSub"] = "";


            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var profile = await _profileService.GetProfileAsync(user);
            if (profile == null)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model, string lang)
        {
            var currentCulture = !string.IsNullOrEmpty(lang) ? lang : CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (model == null || string.IsNullOrEmpty(model.FirstName))
            {
                TempData["ErrorMessageUpdate"] = _translationService.GetTranslation("updateprofile", "errorMessageUpdate", currentCulture);
                return RedirectToAction("UpdateProfile", "Profile");
            }

            var profileIdString = Request.Cookies["SelectedSubProfileId"];
            if (!int.TryParse(profileIdString, out var profileId))
            {
                return BadRequest("No selected profile.");
            }

            var profile = await _profileService.GetProfileByIdAsync(profileId);
            if (profile == null)
            {
                return NotFound();
            }

            var success = await _profileService.UpdateSubProfileAsync(profileId.ToString(), model);
            if (!success)
            {
                TempData["ErrorMessageUpdate"] = _translationService.GetTranslation("updateprofile", "errorMessageUpdate", currentCulture);
                return View(model);
            }

            TempData["StatusMessageUpdate"] = _translationService.GetTranslation("updateprofile", "statusMessageUpdate", currentCulture);
            return RedirectToAction("UpdateProfile", "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSubProfile(string lang)
        {
            var currentCulture = !string.IsNullOrEmpty(lang) ? lang : CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            ViewData["Title"] = _translationService.GetTranslation("subProfileDetails", "title", currentCulture);
            ViewData["Description"] = _translationService.GetTranslation("subProfileDetails", "description", currentCulture);
            ViewData["FirstNameUpdate"] = _translationService.GetTranslation("subProfileDetails", "firstNameUpdate", currentCulture);
            ViewData["SaveButtonUpdate"] = _translationService.GetTranslation("subProfileDetails", "saveButtonUpdate", currentCulture);
            ViewData["ChooseProfileImage"] = _translationService.GetTranslation("subProfileDetails", "chooseProfileImage", currentCulture);
            ViewData["StatusMessageUpdateSub"] = "";
            ViewData["ErrorMessageUpdateSub"] = "";

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var userId = user.Id;

            var profile = await _profileService.GetSelectedProfileAsync(userId);
            if (profile == null)
            {
                return NotFound();
            }

            ViewBag.ProfileId = profile.Id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSubProfile(ProfileViewModel model, string lang)
        {
            var currentCulture = !string.IsNullOrEmpty(lang) ? lang : CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (model == null || string.IsNullOrEmpty(model.FirstName))
            {
                TempData["ErrorMessageUpdateSub"] = _translationService.GetTranslation("subProfileDetails", "errorMessageUpdateSub", currentCulture);
                return RedirectToAction("UpdateSubProfile", "Profile");
            }

            var profileIdString = Request.Cookies["SelectedSubProfileId"];
            if (!int.TryParse(profileIdString, out var profileId))
            {
                return BadRequest("No selected profile.");
            }

            var profile = await _profileService.GetProfileByIdAsync(profileId);
            if (profile == null)
            {
                return NotFound();
            }

            var success = await _profileService.UpdateSubProfileAsync(profileId.ToString(), model);
            if (!success)
            {
                TempData["ErrorMessageUpdateSub"] = _translationService.GetTranslation("subProfileDetails", "errorMessageUpdateSub", currentCulture);
                return View(model);
            }

            TempData["StatusMessageUpdateSub"] = _translationService.GetTranslation("subProfileDetails", "statusMessageUpdateSub", currentCulture);
            return RedirectToAction("UpdateSubProfile", "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string lang)
        {
            var currentCulture = !string.IsNullOrEmpty(lang) ? lang : CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            ViewData["Title"] = _translationService.GetTranslation("details", "title", currentCulture);
            ViewData["Description"] = _translationService.GetTranslation("details", "description", currentCulture);
            ViewData["FirstName"] = _translationService.GetTranslation("details", "firstname", currentCulture);
            ViewData["LastName"] = _translationService.GetTranslation("details", "lastname", currentCulture);
            ViewData["SaveButton"] = _translationService.GetTranslation("details", "saveButton", currentCulture);
            ViewData["StatusMessage"] = "";

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var profile = await _profileService.GetProfileAsync(user);
            if (profile == null)
            {
                return NotFound();
            }
            var model = new UserProfileViewModel
            {
                Firstname = profile.ProfileFirstName,
                Lastname = profile.ProfileLastName
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Details(UserProfileViewModel model, string lang)
        {
            var currentCulture = !string.IsNullOrEmpty(lang) ? lang : CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = _translationService.GetTranslation("details", "errorMessage", currentCulture);
                return RedirectToAction("Details", "Profile");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = _translationService.GetTranslation("details", "errorMessage", currentCulture);
                return RedirectToAction("Details", "Profile");
            }

            if (model == null || string.IsNullOrEmpty(model.Firstname) || string.IsNullOrEmpty(model.Lastname))
            {
                TempData["ErrorMessage"] = _translationService.GetTranslation("details", "errorMessage", currentCulture);
                return RedirectToAction("UpdateProfile", "Profile");
            }

            var success = await _profileService.UpdateProfileAsync(user, model);
            if (!success)
            {
                TempData["ErrorMessage"] = _translationService.GetTranslation("details", "errorMessage", currentCulture);
                return RedirectToAction("Details", "Profile");
            }

            TempData["StatusMessage"] = _translationService.GetTranslation("details", "statusMessage", currentCulture);
            return RedirectToAction("Details", "Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfileImage(ProfileImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var success = await _profileService.UpdateProfileImageAsync(model);
                if (success)
                {
                    return RedirectToAction("UpdateSubProfile", "Profile");
                }
            }

            return View("Error");
        }



        [HttpPost]
        public async Task<IActionResult> DeleteProfile(int profileId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var profile = await _profileService.GetProfileByIdAsync(profileId);
            if (profile == null || profile.UserId != user.Id)
            {
                return NotFound();
            }

            var success = await _profileService.DeleteProfileAsync(profile);
            if (!success)
            {
                ViewData["StatusMessage"] = "Failed to delete profile";
                return View(profile);
            }

            return RedirectToAction("UserProfile", "Profile");
        }


    }
}
