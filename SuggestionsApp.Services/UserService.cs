using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuggestionsApp.Models.Data.Identity;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.Interfaces;

namespace SuggestionsApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApplicationUser> GetUserLoggedIn()
        {
            var currentUser = await GetCurrentUser();
            return currentUser;
        }

        public async Task<string> GetUserIdLoggedIn()
        {
            var currentUser = await GetCurrentUser();
            return currentUser?.Id;
        }

        public async Task<string> GetUserNameLoggedIn()
        {
            var currentUser = await GetCurrentUser();
            return currentUser?.UserName;
        }

        public async Task<IEnumerable<string>> GetUserRolesLoggedIn()
        {
            var roles = await GetCurrentUserRoles();
            return roles;
        }

        public async Task<bool> IsAdminOrModeratorUserLoggedIn()
        {
            var roles = await GetCurrentUserRoles();

            if (roles is null) return false;

            return (roles.Contains("Admin") || roles.Contains("Moderator"));
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        public async Task<string> GetUserNameById(string userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            return appUser.UserName;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }

        #region HelperMethods

            private async Task<ApplicationUser> GetCurrentUser()
            {
                var currentAppUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

                return currentAppUser;
            }

            private async Task<IEnumerable<string>> GetCurrentUserRoles()
            {
                var currentUser = await GetCurrentUser();

                if (currentUser is null) return null;

                var roles = await _userManager.GetRolesAsync(currentUser);

                return roles;
            }

        #endregion
    }
}
