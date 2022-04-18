using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.QueryFilters;

namespace SuggestionsApp.Models.Interfaces
{
    public interface ISuggestionsService
    {
        Task<IEnumerable<Suggestion>> GetSuggestions(bool? isApproved);
        Task<IEnumerable<Suggestion>> GetSearchedSuggestions(bool? isApproved, SuggestionQueryFilter filters);
        Task<Suggestion> GetSuggestionById(int id);
        Task<bool> InsertSuggestion(Suggestion suggestion, string userId, string captchaToken);
        Task<bool> UpdateSuggestion(Suggestion suggestion, string userId);
        Task<bool> DeleteSuggestion(int id, string userId);
        Task<bool> ChangeSuggestionApprovalStatus(int id, bool? approved);

        Task<bool> ChangeSuggestionUpvotesAmount(int id, bool isIncreased);
        Task<bool> SetSuggestionStatus(int id, int stateId);
    }
}
