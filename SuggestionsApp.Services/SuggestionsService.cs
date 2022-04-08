using Microsoft.EntityFrameworkCore;
using SuggestionsApp.Models.Data.Database;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.Exceptions;
using SuggestionsApp.Models.Interfaces;

namespace SuggestionsApp.Services
{
    public class SuggestionsService : ISuggestionsService
    {
        private readonly SuggestionsAppContext _context;

        public SuggestionsService(SuggestionsAppContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Suggestion>> GetSuggestions(bool? isApproved)
        {
            var suggestions = await GetAllSuggestionsQuery()
                                        .Where(p => p.Approved == isApproved)
                                        .ToListAsync();
            return suggestions;
        }

        public async Task<IEnumerable<Suggestion>> GetSearchedSuggestions(bool? isApproved, int? categoryId, int? stateId, string? search)
        {
            var suggestionsQuery = GetAllSuggestionsQuery()
                                        .Where(p => p.Approved == isApproved);

            if (categoryId is not null)
                suggestionsQuery = suggestionsQuery.Where(p => p.CategoryId == categoryId);

            if (stateId is not null)
                suggestionsQuery = suggestionsQuery.Where(p => p.StateId == stateId);

            if (search is not null)
                suggestionsQuery = suggestionsQuery.Where(p => p.Title.Contains(search));

            var suggestions = await suggestionsQuery.ToListAsync();

            return suggestions;
        }

        public async Task<Suggestion> GetSuggestionById(int id)
        {
            var suggestion = await GetAllSuggestionsQuery()
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if(suggestion is null)
            {
                throw new LogicException("La sugerencia que seleccionó no existe o ha sido borrada hace poco.");
            }

            return suggestion;
        }

        public async Task<bool> InsertSuggestion(Suggestion suggestion, string userId)
        {
            if(suggestion is null)
            {
                throw new LogicException("No existe una sugerencia para agregar.");
            }
            
            suggestion.UserId = userId;
            suggestion.UpvotesAmount = 0;
            suggestion.Date = DateTime.Now;

            _context.Add(suggestion);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> UpdateSuggestion(Suggestion suggestion)
        {
            if (suggestion is null)
            {
                throw new LogicException("No existe una sugerencia para editar.");
            }

            var oldSuggestion = await GetSuggestionById(suggestion.Id);
            oldSuggestion.Title = suggestion.Title;
            oldSuggestion.CategoryId = suggestion.CategoryId;
            oldSuggestion.Description = suggestion.Description;

            _context.Update(oldSuggestion);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> DeleteSuggestion(int id)
        {
            var suggestion = await GetSuggestionById(id);

            _context.Suggestions.Remove(suggestion);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> ChangeSuggestionApprovalStatus(int id, bool approved)
        {
            var suggestion = await GetSuggestionById(id);
            suggestion.Approved = approved;

            var result = await UpdateSuggestion(suggestion);

            return result;
        }

        public async Task<bool> SetSuggestionStatus(int id, int stateId)
        {
            var suggestion = await GetSuggestionById(id);
            suggestion.StateId = stateId;

            var result = await UpdateSuggestion(suggestion);

            return result;
        }

        #region HelperMethods

        private IQueryable<Suggestion> GetAllSuggestionsQuery()
        {
            var suggestionsQuery = _context.Suggestions
                                        .Include(p => p.Category)
                                        .Include(p => p.State);

            return suggestionsQuery;
        }

        #endregion
    }
}