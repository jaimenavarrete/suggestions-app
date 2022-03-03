using AutoMapper;
using SuggestionsApp.Models.Data.Identity;
using SuggestionsApp.Models.DataModels;

namespace SuggestionsApp.WebUI.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<ApplicationUser, PersonalUser>();
            CreateMap<PersonalUser, ApplicationUser>();
        }
    }
}
