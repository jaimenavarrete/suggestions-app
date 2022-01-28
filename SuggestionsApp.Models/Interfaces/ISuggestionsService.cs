using SuggestionsApp.Models.DataModels;

namespace SuggestionsApp.Models.Interfaces
{
    public interface ISuggestionsService
    {
        Task<IEnumerable<Suggestion>> GetSuggestions();
        Task<Suggestion> GetSuggestionById(int id);
        Task<Suggestion> InsertSuggestion(Suggestion suggestion);
        Task<Suggestion> UpdateSuggestion(Suggestion suggestion);
        Task<bool> DeleteSuggestion(int id);
    }
}
