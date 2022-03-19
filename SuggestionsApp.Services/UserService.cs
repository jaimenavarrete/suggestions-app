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
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ApplicationUser> GetUserLoggedIn(ClaimsPrincipal principal)
        {
            var currentUser = await _userManager.GetUserAsync(principal);
            return currentUser;
        }

        public async Task<string> GetUserIdLoggedIn(ClaimsPrincipal principal)
        {
            var currentUser = await _userManager.GetUserAsync(principal);
            return currentUser?.Id;
        }

        public async Task<string> GetUserNameLoggedIn(ClaimsPrincipal principal)
        {
            var currentUser = await _userManager.GetUserAsync(principal);
            return currentUser?.UserName;
        }

        public async Task<string> GetUserRoleLoggedIn(ClaimsPrincipal principal)
        {
            var currentUser = await _userManager.GetUserAsync(principal);

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
            var user = await _userManager.FindByIdAsync(userId);
            return user.UserName;
        }

        public async Task<string> GetUserRoleById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null) return "";

            var roles = await _userManager.GetRolesAsync(user);

            return roles.First();
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }

        public async Task<IEnumerable<ApplicationRole>> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles;
        }

        public async Task<bool> InsertUser(ApplicationUser user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded) return false;

            // Add to role
            result = await _userManager.AddToRoleAsync(user, role);

            return result.Succeeded;
        }
    }
}
