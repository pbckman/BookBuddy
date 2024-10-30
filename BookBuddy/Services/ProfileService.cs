using BookBuddy.Data.Contexts;
using BookBuddy.Data.Entities;
using BookBuddy.Models.ViewModels;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookBuddy.Services
{
    public class ProfileService(UserManager<ApplicationUser> userManager, DataContext dataContext)
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly DataContext _dataContext = dataContext;

        public async Task<UserProfileEntity> CreateProfileAsync(string userId, string firstName, string lastName, bool isMainProfile = false)
        {
            var profile = new UserProfileEntity
            {
                UserId = userId,
                ProfileFirstName = firstName,
                ProfileLastName = lastName,
                IsMainProfile = isMainProfile
            };

            _dataContext.Profiles.Add(profile);
            await _dataContext.SaveChangesAsync();
            return profile;
        }
        public async Task<UserProfileEntity?> GetProfileAsync(ApplicationUser user)
        {

            return await _dataContext.Profiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
        }

        public async Task<UserProfileEntity?> GetProfileByIdAsync(int profileId)
        {
            return await _dataContext.Profiles.FirstOrDefaultAsync(p => p.Id == profileId);
        }

        public async Task<bool> UpdateProfileAsync(ApplicationUser user, ProfileViewModel model)
        {
            var profile = await GetProfileAsync(user);
            if (profile == null) return false;

            profile.ProfileFirstName = model.FirstName;
            profile.ProfileLastName = model.LastName;

            _dataContext.Profiles.Update(profile);
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
