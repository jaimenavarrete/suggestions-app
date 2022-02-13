using SuggestionsApp.Models.DataModels;

namespace SuggestionsApp.Models.Interfaces
{
    public interface ISuggestionsService
    {
        Task<IEnumerable<Suggestion>> GetSuggestions(bool? isApproved);
        Task<IEnumerable<Suggestion>> GetSearchedSuggestions(bool? isApproved, int? categoryId, int? stateId, string? search);
        Task<Suggestion?> GetSuggestionById(int id);
        Task<bool> InsertSuggestion(Suggestion suggestion);
        Task<bool> UpdateSuggestion(Suggestion suggestion);
        Task<bool> DeleteSuggestion(int id);
    }
}
