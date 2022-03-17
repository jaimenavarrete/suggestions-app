using SuggestionsApp.Models.Data.Identity;
using System.Security.Claims;

namespace SuggestionsApp.Models.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserLoggedIn(ClaimsPrincipal principal);

        Task<string> GetUserNameLoggedIn(ClaimsPrincipal principal);

        Task<string> GetUserIdLoggedIn(ClaimsPrincipal principal);

        Task<string> GetUserRoleLoggedIn(ClaimsPrincipal principal);

        Task<ApplicationUser> GetUserById(string userId);

        Task<string> GetUserNameById(string userId);

        Task<IEnumerable<ApplicationUser>> GetAllUsers();
    }
}
