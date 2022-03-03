using AutoMapper;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.WebUI.ViewModels;

namespace SuggestionsApp.WebUI.Mappings
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

    public class ViewSuggestionMapping : Profile
    {
        public ViewSuggestionMapping()
        {
            CreateMap<Suggestion, ViewSuggestionViewModel>();
            CreateMap<ViewSuggestionViewModel, Suggestion>();
        }
    }

    public class PendingSuggestionMapping : Profile
    {
        public PendingSuggestionMapping()
        {
            CreateMap<Suggestion, PendingSuggestionViewModel>();
            CreateMap<PendingSuggestionViewModel, Suggestion>();
        }
    }
}
