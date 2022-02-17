using Microsoft.EntityFrameworkCore;
using SuggestionsApp.Models.Data.Database;
using SuggestionsApp.Models.Data.Identity;
using SuggestionsApp.Models.DataModels;
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

        public async Task<Suggestion?> GetSuggestionById(int id)
        {
            var suggestion = await GetAllSuggestionsQuery()
                                        .FirstOrDefaultAsync(p => p.Id == id);

            return suggestion;
        }

        public async Task<bool> InsertSuggestion(Suggestion suggestion)
        {
            _context.Add(suggestion);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> UpdateSuggestion(Suggestion suggestion)
        {
            _context.Update(suggestion);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> DeleteSuggestion(int id)
        {
            var suggestion = await GetSuggestionById(id);

            if(suggestion is not null)
            {
                _context.Suggestions.Remove(suggestion);
                var affectedRows = await _context.SaveChangesAsync();

                return affectedRows > 0;
            }

            return false;
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