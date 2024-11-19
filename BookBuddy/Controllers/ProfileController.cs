using BookBuddy.Business.Services.AccountService;
using BookBuddy.Business.Services.TranslationService;
using BookBuddy.Models.ViewModels;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            await _profileService.CreateProfileAsync(user.Id, model.FirstName, model.LastName, isMainProfile: false);



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
            ViewData["TitleCreate"] = _translationService.GetTranslation("updateprofile", "titleCreate", lang);
            ViewData["DescriptionCreate"] = _translationService.GetTranslation("updateprofile", "descriptionCreate", lang);
            ViewData["FirstNameCreate"] = _translationService.GetTranslation("updateprofile", "firstNameCreate", lang);
            ViewData["SaveButtonCreate"] = _translationService.GetTranslation("updateprofile", "saveButtonCreate", lang);
            ViewData["TitleUpdate"] = _translationService.GetTranslation("updateprofile", "titleUpdate", lang);
            ViewData["DescriptionUpdate"] = _translationService.GetTranslation("updateprofile", "descriptionUpdate", lang);
            ViewData["FirstNameUpdate"] = _translationService.GetTranslation("updateprofile", "firstNameUpdate", lang);
            ViewData["SaveButtonUpdate"] = _translationService.GetTranslation("updateprofile", "saveButtonUpdate", lang);


            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var userId = user.Id;

            var profile = await _profileService.GetSubProfileAsync(userId);
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
                TempData["StatusMessage"] = "Could not update profile, please try again later.";
                return View(model);
            }

            TempData["StatusMessage"] = "Profile updated successfully!";
            return RedirectToAction("UpdateProfile", "Profile");
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
                TempData["ErrorMessage"] = "User updated successfully!";
                return RedirectToAction("Details", "Profile");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User updated successfully!";
                return RedirectToAction("Details", "Profile");
            }

            var success = await _profileService.UpdateProfileAsync(user, model);
            if (!success)
            {
                TempData["ErrorMessage"] = "Could not update user, please try again later.";
                return RedirectToAction("Details", "Profile");
            }

            TempData["StatusMessage"] = "User updated successfully!";
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
