using SuggestionsApp.Models.Data.Identity;
using SuggestionsApp.Models.DataModels;

namespace SuggestionsApp.Models.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserLoggedIn();

        Task<string> GetUserNameLoggedIn();

        Task<string> GetUserIdLoggedIn();

        Task<IEnumerable<string>> GetUserRolesLoggedIn();

        Task<bool> IsAdminOrModeratorUserLoggedIn();

        Task<ApplicationUser> GetUserById(string userId);

        Task<string> GetUserNameById(string userId);

        Task<IEnumerable<ApplicationUser>> GetAllUsers();
    }
}
