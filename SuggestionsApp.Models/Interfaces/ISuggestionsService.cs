using SuggestionsApp.Models.DataModels;

namespace SuggestionsApp.Models.Interfaces
{
    public interface ISuggestionsService
    {
        Task<IEnumerable<Suggestion>> GetSuggestions();
        Task<Suggestion> GetSuggestionById(int id);
        Task<bool> InsertSuggestion(Suggestion suggestion);
        Task<bool> UpdateSuggestion(Suggestion suggestion);
        Task<bool> DeleteSuggestion(int id);
    }
}
