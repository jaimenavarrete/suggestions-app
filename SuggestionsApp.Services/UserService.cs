using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuggestionsApp.Models.Data.Identity;
using SuggestionsApp.Models.Interfaces;
using System.Security.Claims;
using SuggestionsApp.Models.Exceptions;

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

        public async Task<string> GetLoggedUserId(ClaimsPrincipal principal)
        {
            var currentUser = await _userManager.GetUserAsync(principal);
            return currentUser is null ? "" : currentUser.Id;
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new LogicException("El usuario que seleccionó no existe.");
            }
            
            return user;
        }

        public async Task<string> GetUserNameById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user is null ? "Anónimo" : user.UserName;
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
            user.EmailConfirmed = true;
            
            var result = await _userManager.CreateAsync(user, password);
            
            if (!result.Succeeded) return false;

            // Add to role
            result = await _userManager.AddToRoleAsync(user, role);
            
            return result.Succeeded;
        }

        public async Task<bool> ChangeUserLockState(string userId)
        {
            var user = await GetUserById(userId);
            var isUserLocked = user.LockoutEnd > DateTime.UtcNow;
            var lockDate = isUserLocked ? DateTime.UtcNow : DateTimeOffset.MaxValue;

            if (!user.LockoutEnabled)
            {
                throw new BusinessException("No se puede bloquear la cuenta de administrador principal");
            }
            
            var result = await _userManager.SetLockoutEndDateAsync(user, lockDate);

            return result.Succeeded;
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var user = await GetUserById(userId);

            if (!user.LockoutEnabled)
            {
                throw new BusinessException("No se puede borrar la cuenta de administrador principal");
            }

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }
    }
}
