using BookBuddy.Data.Contexts;
using BookBuddy.Data.Entities;
using BookBuddy.Models.ViewModels;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookBuddy.Business.Services.AccountService
{
    public class ProfileService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<UserProfileEntity> CreateProfileAsync(string userId, string firstName, string lastName, bool isMainProfile = false)
        {
            var profile = new UserProfileEntity
            {
                UserId = userId,
                ProfileFirstName = firstName,
                ProfileLastName = lastName,
                IsMainProfile = isMainProfile,
            };

            _dataContext.Profiles.Add(profile);
            await _dataContext.SaveChangesAsync();
            return profile;
        }

        public async Task<UserProfileEntity> CreateSubProfileAsync(string userId, string firstName, string lastName, bool isMainProfile = false)
        {
            var profile = new UserProfileEntity
            {
                UserId = userId,
                ProfileFirstName = firstName,
                ProfileLastName = lastName,
                IsMainProfile = isMainProfile,
                ProfileImage = "bunny.webp"
            };

            _dataContext.Profiles.Add(profile);
            await _dataContext.SaveChangesAsync();
            return profile;
        }
        public async Task<UserProfileEntity?> GetProfileAsync(ApplicationUser user)
        {

            return await _dataContext.Profiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
        }


        public async Task<IEnumerable<UserProfileEntity>?> GetAllProfilesAsync(string userId)
        {

            return await _dataContext.Profiles
                                     .Where(profile => profile.UserId == userId)
                                     .ToListAsync();
        }

        public async Task<UserProfileEntity> GetMainProfileAsync(string userId)
        {

            return await _dataContext.Profiles.FirstOrDefaultAsync(profile => profile.UserId == userId && profile.IsMainProfile == true);

        }

        public async Task<UserProfileEntity> GetSubProfileAsync(string userId)
        {

            return await _dataContext.Profiles.FirstOrDefaultAsync(profile => profile.UserId == userId && profile.IsMainProfile != true);
        }

        public async Task<UserProfileEntity> GetSubsProfileAsync(string profileId)
        {

            var convertedId = Convert.ToInt32(profileId);

            var profile = await _dataContext.Profiles.FirstOrDefaultAsync(profile => profile.Id == convertedId && profile.IsMainProfile != true);

            return profile;
        }
        public async Task<UserProfileEntity> GetSelectedProfileAsync()
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("SelectedProfileId", out string profileIdValue))
            {
                if (int.TryParse(profileIdValue, out int profileId))
                {
                    return await _dataContext.Profiles.FirstOrDefaultAsync(p => p.Id == profileId);
                }

            }

            return null!;
        }

        public async Task<IEnumerable<UserProfileEntity>> GetSubProfilesAsync(string userId)
        {

            return await _dataContext.Profiles
                                    .Where(profile => profile.UserId == userId && profile.IsMainProfile != true)
                                    .ToListAsync();
        }

        public async Task<UserProfileEntity?> GetProfileByIdAsync(int profileId)
        {
            return await _dataContext.Profiles.FirstOrDefaultAsync(p => p.Id == profileId);
        }

        public async Task<UserProfileEntity?> GetProfileByIdAsync(string profileId)
        {
            var convertedId = Convert.ToInt32(profileId);

            return await _dataContext.Profiles.FirstOrDefaultAsync(p => p.Id == convertedId);
        }

        public async Task<UserProfileEntity> GetSelectedProfileAsync(string userId)
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("SelectedProfileId", out string profileIdValue))
            {
                if (int.TryParse(profileIdValue, out int profileId))
                {
                    return await _dataContext.Profiles.FirstOrDefaultAsync(p => p.Id == profileId);
                }

            }

            return await GetMainProfileAsync(userId);
        }

        public async Task<UserProfileEntity> GetSelectedSubProfileAsync(string userId)
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("SelectedSubProfileId", out string profileIdValue))
            {
                if (int.TryParse(profileIdValue, out int profileId))
                {
                    return await _dataContext.Profiles.FirstOrDefaultAsync(p => p.Id == profileId);
                }

            }
            return null;
        }

        public async Task<bool> UpdateProfileAsync(ApplicationUser user, UserProfileViewModel model)
        {
            var profile = await GetProfileAsync(user);
            if (profile == null) return false;


            profile.ProfileFirstName = model.Firstname;
            profile.ProfileLastName = model.Lastname;

            _dataContext.Profiles.Update(profile);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateSubProfileAsync(string profileId, ProfileViewModel model)
        {
            var profile = await GetSubsProfileAsync(profileId);
            if (profile == null) return false;

            profile.ProfileFirstName = model.FirstName;

            _dataContext.Profiles.Update(profile);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateProfileImageAsync(ProfileImageViewModel model)
        {
            var profile = await GetProfileByIdAsync(model.ProfileId);
            if (profile == null) return false;

            profile.ProfileImage = model.SelectedAvatar;

            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProfileAsync(UserProfileEntity profile)
        {
            try
            {
                _dataContext.Profiles.Remove(profile);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch
            {

                return false;
            }
        }
    }
}
