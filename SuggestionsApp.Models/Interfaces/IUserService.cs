using SuggestionsApp.Models.DataModels;

namespace SuggestionsApp.Models.Interfaces
{
    public interface IUserService
    {
        Task<PersonalUser> GetUserLoggedIn();

        Task<string> GetUserNameLoggedIn();

        Task<string> GetUserIdLoggedIn();

        Task<PersonalUser> GetUserById(string userId);

        Task<string> GetUserNameById(string userId);
    }
}
