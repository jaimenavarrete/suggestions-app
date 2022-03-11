using SuggestionsApp.Models.DataModels;

namespace SuggestionsApp.Models.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserLoggedIn();

        Task<string> GetUserNameLoggedIn();

        Task<string> GetUserIdLoggedIn();

        Task<User> GetUserById(string userId);

        Task<string> GetUserNameById(string userId);
    }
}
