using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        public async Task<PersonalUser> GetUserLoggedIn()
        {
            ApplicationUser currentUser = await GetCurrentUser();

            var personalUser = _mapper.Map<PersonalUser>(currentUser);

            return personalUser;
        }

        public async Task<string> GetUserIdLoggedIn()
        {
            ApplicationUser currentUser = await GetCurrentUser();

            return currentUser.Id;
        }

        public async Task<string> GetUserNameLoggedIn()
        {
            ApplicationUser currentUser = await GetCurrentUser();

            return currentUser.UserName;
        }

        public async Task<PersonalUser> GetUserById(string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            var personalUser = _mapper.Map<PersonalUser>(user);

            return personalUser;
        }

        public async Task<string> GetUserNameById(string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            return user.UserName;
        }

        #region HelperMethods

            private async Task<ApplicationUser> GetCurrentUser()
            {
                var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

                return currentUser;
            }

        #endregion
    }
}
