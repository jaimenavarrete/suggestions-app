using AutoMapper;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.ViewModels;

namespace SuggestionsApp.Models.Mappings
{
    public class SuggestionMapping : Profile
    {
        public SuggestionMapping()
        {
            CreateMap<Suggestion, SuggestionViewModel>();
            CreateMap<SuggestionViewModel, Suggestion>();
        }
    }

    public class SuggestionFormMapping : Profile
    {
        public SuggestionFormMapping()
        {
            CreateMap<Suggestion, SuggestionFormViewModel>();
            CreateMap<SuggestionFormViewModel, Suggestion>();
        }
    }

    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();
        }
    }

    public class StateMapping : Profile
    {
        public StateMapping()
        {
            CreateMap<State, StateViewModel>();
            CreateMap<StateViewModel, State>();
        }
    }
}
