using SuggestionsApp.Models.DataModels;

namespace SuggestionsApp.Models.Interfaces
{
    public interface IUpvotesService
    {
        Task<Upvote?> GetSuggestionUserUpvote(int suggestionId, string userId);

        Task<bool> InsertUpvote(int suggestionId, string userId);

        Task<bool> DeleteUpvote(int suggestionId, string userId);
    }
}
