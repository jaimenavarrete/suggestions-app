using AutoMapper;
using SuggestionsApp.Models.Data.Identity;
using SuggestionsApp.Models.DataModels;

namespace SuggestionsApp.Models.Mappings
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
