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
        private readonly IMapper _mapper;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<User> GetUserLoggedIn()
        {
            var currentAppUser = await GetCurrentUser();

            var user = _mapper.Map<User>(currentAppUser);

            return user;
        }

        public async Task<string> GetUserIdLoggedIn()
        {
            var currentAppUser = await GetCurrentUser();

            if(currentAppUser == null)
            {
                return null;
            }

            return currentAppUser.Id;
        }

        public async Task<string> GetUserNameLoggedIn()
        {
            var currentAppUser = await GetCurrentUser();

            return currentAppUser.UserName;
        }

        public async Task<User> GetUserById(string userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId);

            var user = _mapper.Map<User>(appUser);

            return user;
        }

        public async Task<string> GetUserNameById(string userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId);

            return appUser.UserName;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var appUsers = await _userManager.Users.ToListAsync();

            var users = _mapper.Map<IEnumerable<User>>(appUsers);

            return users;
        }

        #region HelperMethods

            private async Task<ApplicationUser> GetCurrentUser()
            {
                var currentAppUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

                return currentAppUser;
            }

        #endregion
    }
}
