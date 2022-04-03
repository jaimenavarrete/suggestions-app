using Microsoft.EntityFrameworkCore;
using SuggestionsApp.Models.Data.Database;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.Exceptions;
using SuggestionsApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuggestionsApp.Services
{
    public class UpvotesService : IUpvotesService
    {
        private readonly SuggestionsAppContext _context;
        private readonly ISuggestionsService _suggestionsService;

        public UpvotesService(SuggestionsAppContext context, ISuggestionsService suggestionsService)
        {
            _context = context;
            _suggestionsService = suggestionsService;
        }

        public async Task<Upvote> GetSuggestionUserUpvote(int suggestionId, string userId)
        {
            var upvote = await _context.Upvotes
                                    .FirstOrDefaultAsync(p => p.SuggestionId == suggestionId && p.UserId == userId);

            return upvote;
        }

        public async Task<bool> InsertUpvote(int suggestionId, string userId)
        {
            if (suggestionId == 0 || string.IsNullOrEmpty(userId))
            {
                throw new LogicException("No existe un voto positivo para agregar.");
            }
            
            var upvote = new Upvote(suggestionId, userId);
            
            _context.Add(upvote);
            var affectedRows = await _context.SaveChangesAsync();

            if(affectedRows > 0)
            {
                var suggestion = await _suggestionsService.GetSuggestionById(upvote.SuggestionId);
                suggestion.UpvotesAmount++;
                var succeeded = await _suggestionsService.UpdateSuggestion(suggestion);

                return succeeded;
            }

            return affectedRows > 0;
        }

        public async Task<bool> DeleteUpvote(int suggesstionId, string userId)
        {
            var upvote = await GetSuggestionUserUpvote(suggesstionId, userId);

            if (upvote is null)
            {
                throw new LogicException("No existe un voto positivo para borrar.");
            }

            _context.Upvotes.Remove(upvote);
            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows > 0)
            {
                var suggestion = await _suggestionsService.GetSuggestionById(upvote.SuggestionId);
                suggestion.UpvotesAmount--;
                var succeeded = await _suggestionsService.UpdateSuggestion(suggestion);

                return succeeded;
            }

            return affectedRows > 0;
        }
    }
}
