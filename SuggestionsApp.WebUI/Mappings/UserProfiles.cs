using AutoMapper;
using SuggestionsApp.Models.Data.Identity;
using SuggestionsApp.WebUI.ViewModels;

namespace SuggestionsApp.WebUI.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<ApplicationUser, UserViewModel>();
            CreateMap<UserViewModel, ApplicationUser>();
        }
    }

    public class UsersListMapping : Profile
    {
        public UsersListMapping()
        {
            CreateMap<ApplicationUser, UsersListViewModel>();
            CreateMap<UsersListViewModel, ApplicationUser>();
        }
    }

    public class RoleMapping : Profile
    {
        public RoleMapping()
        {
            CreateMap<ApplicationRole, RoleViewModel>();
            CreateMap<RoleViewModel, ApplicationRole>();
        }
    }
}
