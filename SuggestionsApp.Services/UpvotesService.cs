using Microsoft.EntityFrameworkCore;
using SuggestionsApp.Models.Data.Database;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.Exceptions;
using SuggestionsApp.Models.Interfaces;

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

        public async Task<Upvote?> GetSuggestionUserUpvote(int suggestionId, string userId)
        {
            var upvote = await _context.Upvotes
                                    .FirstOrDefaultAsync(p => p.SuggestionId == suggestionId && p.UserId == userId);

            return upvote;
        }
        
        public async Task<bool> SuggestionHasUserUpvote(int suggestionId, string userId)
        {
            var upvote = await GetSuggestionUserUpvote(suggestionId, userId);
            return upvote is not null;
        }

        public async Task<bool> InsertUpvote(Upvote upvote)
        {
            if (upvote is null || upvote.SuggestionId == 0 || string.IsNullOrEmpty(upvote.UserId))
            {
                throw new LogicException("No existe un voto positivo para agregar.");
            }
            
            var suggestion = await _suggestionsService.GetSuggestionById(upvote.SuggestionId);

            if (suggestion.UserId == upvote.UserId)
            {
                throw new BusinessException("No puede votar una sugerencia que usted ha creado.");
            }

            _context.Add(upvote);
            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows == 0) return false;
            
            suggestion.UpvotesAmount++;
            var succeeded = await _suggestionsService.UpdateSuggestion(suggestion, suggestion.UserId);

            return succeeded;
        }

        public async Task<bool> DeleteUpvote(Upvote upvote)
        {
            if (upvote is null)
            {
                throw new LogicException("No existe un voto positivo para borrar.");
            }

            _context.Remove(upvote);
            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows == 0) return false;

            var suggestion = await _suggestionsService.GetSuggestionById(upvote.SuggestionId);
            suggestion.UpvotesAmount--;
            var succeeded = await _suggestionsService.UpdateSuggestion(suggestion, suggestion.UserId);

            return succeeded;
        }
    }
}
