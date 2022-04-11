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
                throw new LogicException("La sugerencia que seleccionó no existe.");
            }

            return suggestion;
        }

        public async Task<bool> InsertSuggestion(Suggestion suggestion, string userId)
        {
            if(suggestion is null)
            {
                throw new LogicException("No existe una sugerencia para agregar.");
            }

            if (string.IsNullOrEmpty(suggestion.Title) || suggestion.CategoryId == 0)
            {
                throw new BusinessException("Debe agregar todos los campos requeridos.");
            }

            if (suggestion.Title.Length > 100 || suggestion.Description?.Length > 1000)
            {
                throw new BusinessException("El título debe tener 100 caracteres o menos y la descripción debe tener 1000 caracteres o menos.");
            }

            suggestion.UserId = userId;
            suggestion.UpvotesAmount = 0;
            suggestion.Date = DateTime.Now;

            _context.Add(suggestion);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> UpdateSuggestion(Suggestion suggestion, string userId)
        {
            if (suggestion is null)
            {
                throw new LogicException("No existe una sugerencia para editar.");
            }
            
            if (string.IsNullOrEmpty(suggestion.Title) || suggestion.CategoryId == 0)
            {
                throw new BusinessException("Debe agregar todos los campos requeridos.");
            }

            if (suggestion.Title.Length > 100 || suggestion.Description?.Length > 1000)
            {
                throw new BusinessException("El título debe tener 100 caracteres o menos y la descripción debe tener 1000 caracteres o menos.");
            }

            var existingSuggestion = await GetSuggestionById(suggestion.Id);

            if (existingSuggestion.UserId != userId)
            {
                throw new BusinessException("No puede editar la sugerencia de otro usuario.");
            }
            
            existingSuggestion.Title = suggestion.Title;
            existingSuggestion.CategoryId = suggestion.CategoryId;
            existingSuggestion.Description = suggestion.Description;

            _context.Update(existingSuggestion);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> DeleteSuggestion(int id, string userId)
        {
            var suggestion = await GetSuggestionById(id);

            if (suggestion.UserId != userId)
            {
                throw new BusinessException("No puede borrar la sugerencia de otro usuario.");
            }

            _context.Remove(suggestion);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> ChangeSuggestionApprovalStatus(int id, bool approved)
        {
            var suggestion = await GetSuggestionById(id);
            suggestion.Approved = approved;

            var result = await UpdateSuggestion(suggestion, suggestion.UserId);

            return result;
        }

        public async Task<bool> SetSuggestionStatus(int id, int stateId)
        {
            var suggestion = await GetSuggestionById(id);
            suggestion.StateId = stateId;

            var result = await UpdateSuggestion(suggestion, suggestion.UserId);

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