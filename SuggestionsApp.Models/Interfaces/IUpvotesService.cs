using SuggestionsApp.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuggestionsApp.Models.Interfaces
{
    public interface IUpvotesService
    {
        Task<int> GetUpvotesAmountBySuggestionId(int id);

        Task<Upvote> GetSuggestionUserUpvote(int suggesstionId, string userId);

        Task<bool> IsSuggestionUserUpvoteActive(int suggesstionId, string userId);

        Task<bool> InsertUpvote(Upvote upvote);

        Task<bool> DeleteUpvote(int suggesstionId, string userId);
    }
}
