using AutoMapper;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.ViewModels;

namespace SuggestionsApp.Models.Mappings
{
    public class StateMapping : Profile
    {
        public StateMapping()
        {
            CreateMap<State, StateViewModel>();
            CreateMap<StateViewModel, State>();
        }
    }
}
