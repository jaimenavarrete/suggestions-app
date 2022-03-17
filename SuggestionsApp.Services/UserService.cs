using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuggestionsApp.Models.Data.Identity;
using SuggestionsApp.Models.Interfaces;
using System.Security.Claims;

namespace SuggestionsApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserLoggedIn(ClaimsPrincipal principal)
        {
            var currentUser = await GetCurrentUser(principal);
            return currentUser;
        }

        public async Task<string> GetUserIdLoggedIn(ClaimsPrincipal principal)
        {
            var currentUser = await GetCurrentUser(principal);
            return currentUser?.Id;
        }

        public async Task<string> GetUserNameLoggedIn(ClaimsPrincipal principal)
        {
            var currentUser = await GetCurrentUser(principal);
            return currentUser?.UserName;
        }

        public async Task<string> GetUserRoleLoggedIn(ClaimsPrincipal principal)
        {
            var currentUser = await GetCurrentUser(principal);

            if (currentUser is null) return "";

            var roles = await _userManager.GetRolesAsync(currentUser);

            return roles.First();
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

            private async Task<ApplicationUser> GetCurrentUser(ClaimsPrincipal principal)
            {
                var currentUser = await _userManager.GetUserAsync(principal);
                return currentUser;
            }

        #endregion
    }
}
