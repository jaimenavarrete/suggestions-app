using SuggestionsApp.Models.DataModels;

namespace SuggestionsApp.Models.Interfaces
{
    public interface ISuggestionsService
    {
        Task<IEnumerable<Suggestion>> GetSuggestions(bool? isApproved);
        Task<IEnumerable<Suggestion>> GetSearchedSuggestions(bool? isApproved, int? categoryId, int? stateId, string? search);
        Task<Suggestion> GetSuggestionById(int id);
        Task<bool> InsertSuggestion(Suggestion suggestion, string userId);
        Task<bool> UpdateSuggestion(Suggestion suggestion, string userId);
        Task<bool> DeleteSuggestion(int id, string userId);
        Task<bool> ChangeSuggestionApprovalStatus(int id, bool approved);
        Task<bool> SetSuggestionStatus(int id, int stateId);
    }
}
