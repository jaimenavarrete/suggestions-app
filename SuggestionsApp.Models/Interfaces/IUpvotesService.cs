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
        Task<Upvote> GetSuggestionUserUpvote(int suggesstionId, string userId);

        Task<bool> InsertUpvote(Upvote upvote);

        Task<bool> DeleteUpvote(int suggesstionId, string userId);
    }
}
