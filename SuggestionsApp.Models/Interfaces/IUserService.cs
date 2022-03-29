using SuggestionsApp.Models.Data.Identity;
using System.Security.Claims;

namespace SuggestionsApp.Models.Interfaces
{
    public interface IUserService
    {
        Task<string> GetLoggedUserId(ClaimsPrincipal principal);

        Task<ApplicationUser> GetUserById(string userId);

        Task<string> GetUserNameById(string userId);

        Task<string> GetUserRoleById(string userId);

        Task<IEnumerable<ApplicationUser>> GetUsers();

        Task<IEnumerable<ApplicationRole>> GetRoles();

        Task<bool> InsertUser(ApplicationUser user, string password, string role);

        Task<bool> ChangeUserLockState(string userId);
    }
}
