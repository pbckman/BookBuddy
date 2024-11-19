using BookBuddy.Business.Services.AccountService;
using BookBuddy.Business.Services.TranslationService;
using BookBuddy.Models.ViewModels;
using EPiServer.Cms.UI.AspNetIdentity;
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


        [HttpGet]
        public IActionResult UserProfile(string lang = "en")
        {
            ViewData["Title"] = _translationService.GetTranslation("userprofile", "title", lang);
            ViewData["Description"] = _translationService.GetTranslation("userprofile", "description", lang);
            ViewData["FirstName"] = _translationService.GetTranslation("userprofile", "firstname", lang);
            ViewData["SaveButton"] = _translationService.GetTranslation("userprofile", "saveButton", lang);

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateProfile(ProfileViewModel model, string lang = "en")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (model == null || string.IsNullOrEmpty(model.FirstName))
            {
                TempData["ErrorMessageCreate"] = _translationService.GetTranslation("updateprofile", "errorMessageCreate", lang);
                return RedirectToAction("UpdateProfile", "Profile", new { lang });
            }

            await _profileService.CreateProfileAsync(user.Id, model.FirstName, model.LastName, isMainProfile: false);

            TempData["StatusMessageCreate"] = _translationService.GetTranslation("updateprofile", "statusMessageCreate", lang);
            return RedirectToAction("UpdateProfile", "Profile", new { lang });
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
        public async Task<IActionResult> UpdateProfile(string lang = "en")
        {
            ViewData["Title"] = _translationService.GetTranslation("updateprofile", "title", lang);
            ViewData["Description"] = _translationService.GetTranslation("updateprofile", "description", lang);
            ViewData["FirstNameCreate"] = _translationService.GetTranslation("updateprofile", "firstNameCreate", lang);
            ViewData["SaveButtonCreate"] = _translationService.GetTranslation("updateprofile", "saveButtonCreate", lang);
            ViewData["TitleUpdate"] = _translationService.GetTranslation("updateprofile", "titleUpdate", lang);
            ViewData["DescriptionUpdate"] = _translationService.GetTranslation("updateprofile", "descriptionUpdate", lang);
            ViewData["FirstNameUpdate"] = _translationService.GetTranslation("updateprofile", "firstNameUpdate", lang);
            ViewData["SaveButtonUpdate"] = _translationService.GetTranslation("updateprofile", "saveButtonUpdate", lang);
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
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model, string lang = "en")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (model == null || string.IsNullOrEmpty(model.FirstName))
            {
                TempData["ErrorMessageUpdate"] = _translationService.GetTranslation("updateprofile", "errorMessageUpdate", lang);
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
                TempData["ErrorMessageUpdate"] = _translationService.GetTranslation("updateprofile", "errorMessageUpdate", lang);
                return View(model);
            }

            TempData["StatusMessageUpdate"] = _translationService.GetTranslation("updateprofile", "statusMessageUpdate", lang);
            return RedirectToAction("UpdateProfile", "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSubProfile(string lang = "en")
        {
            ViewData["Title"] = _translationService.GetTranslation("subProfileDetails", "title", lang);
            ViewData["Description"] = _translationService.GetTranslation("subProfileDetails", "description", lang);
            ViewData["FirstNameUpdate"] = _translationService.GetTranslation("subProfileDetails", "firstNameUpdate", lang);
            ViewData["SaveButtonUpdate"] = _translationService.GetTranslation("subProfileDetails", "saveButtonUpdate", lang);
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
        public async Task<IActionResult> UpdateSubProfile(ProfileViewModel model, string lang = "en")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (model == null || string.IsNullOrEmpty(model.FirstName))
            {
                TempData["ErrorMessageUpdateSub"] = _translationService.GetTranslation("subProfileDetails", "errorMessageUpdateSub", lang);
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
                TempData["ErrorMessageUpdateSub"] = _translationService.GetTranslation("subProfileDetails", "errorMessageUpdateSub", lang);
                return View(model);
            }

            TempData["StatusMessageUpdateSub"] = _translationService.GetTranslation("subProfileDetails", "statusMessageUpdateSub", lang);
            return RedirectToAction("UpdateSubProfile", "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string lang = "en")
        {
            ViewData["Title"] = _translationService.GetTranslation("details", "title", lang);
            ViewData["Description"] = _translationService.GetTranslation("details", "description", lang);
            ViewData["FirstName"] = _translationService.GetTranslation("details", "firstname", lang);
            ViewData["LastName"] = _translationService.GetTranslation("details", "firstname", lang);
            ViewData["Email"] = _translationService.GetTranslation("details", "firstname", lang);
            ViewData["SaveButton"] = _translationService.GetTranslation("details", "saveButton", lang);
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
        public async Task<IActionResult> Details(UserProfileViewModel model, string lang = "en")
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = _translationService.GetTranslation("details", "errorMessage", lang);
                return RedirectToAction("Details", "Profile");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = _translationService.GetTranslation("details", "errorMessage", lang);
                return RedirectToAction("Details", "Profile");
            }

            if (model == null || string.IsNullOrEmpty(model.Firstname) || string.IsNullOrEmpty(model.Lastname))
            {
                TempData["ErrorMessage"] = _translationService.GetTranslation("details", "errorMessage", lang);
                return RedirectToAction("UpdateProfile", "Profile");
            }

            var success = await _profileService.UpdateProfileAsync(user, model);
            if (!success)
            {
                TempData["ErrorMessage"] = _translationService.GetTranslation("details", "errorMessage", lang);
                return RedirectToAction("Details", "Profile");
            }

            TempData["StatusMessage"] = _translationService.GetTranslation("details", "statusMessage", lang);
            return RedirectToAction("Details", "Profile");
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
