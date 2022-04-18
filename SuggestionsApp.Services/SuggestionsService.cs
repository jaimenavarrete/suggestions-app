using Microsoft.EntityFrameworkCore;
using SuggestionsApp.Models.Data.Database;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.Exceptions;
using SuggestionsApp.Models.Interfaces;
using SuggestionsApp.Models.QueryFilters;
using SuggestionsApp.Services.Shared;

namespace SuggestionsApp.Services
{
    public class SuggestionsService : ISuggestionsService
    {
        private readonly SuggestionsAppContext _context;
        private readonly IUserService _userService;

        public SuggestionsService(SuggestionsAppContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        
        public async Task<IEnumerable<Suggestion>> GetSuggestions(bool? isApproved)
        {
            var suggestions = await GetAllSuggestionsQuery()
                                        .Where(p => p.Approved == isApproved)
                                        .ToListAsync();

            return suggestions;
        }

        public async Task<IEnumerable<Suggestion>> GetSearchedSuggestions(bool? isApproved, SuggestionQueryFilter filters)
        {
            var suggestionsQuery = GetAllSuggestionsQuery()
                                        .Where(p => p.Approved == isApproved || p.UserId == filters.UserId);

            if (filters.CategoryId is not null)
                suggestionsQuery = suggestionsQuery.Where(p => p.CategoryId == filters.CategoryId);
            
            if (filters.StateId is not null)
                suggestionsQuery = suggestionsQuery.Where(p => p.StateId == filters.StateId);
            
            if (filters.SearchText is not null)
                suggestionsQuery = suggestionsQuery.Where(p => p.Title.Contains(filters.SearchText));

            suggestionsQuery = filters.OrderBy switch
            {
                OrderBy.DateAsc => suggestionsQuery.OrderBy(p => p.Date),
                OrderBy.DateDesc => suggestionsQuery.OrderByDescending(p => p.Date),
                OrderBy.PopularityAsc => suggestionsQuery.OrderBy(p => p.UpvotesAmount),
                OrderBy.PopularityDesc => suggestionsQuery.OrderByDescending(p => p.UpvotesAmount),
                _ => suggestionsQuery
            };

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

        public async Task<bool> InsertSuggestion(Suggestion suggestion, string userId, string captchaToken)
        {
            await ValidateCaptchaField(captchaToken);
            ValidateSuggestionFields(suggestion);
            
            var hasAdministrationRole = await _userService.IsUserInAdministrationRole(userId);

            suggestion.UserId = userId;
            suggestion.UpvotesAmount = 0;
            suggestion.Date = DateTime.Now;
            suggestion.Approved = hasAdministrationRole ? true : null;

            _context.Add(suggestion);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> UpdateSuggestion(Suggestion suggestion, string userId)
        {
            ValidateSuggestionFields(suggestion);
            
            var hasAdministrationRole = await _userService.IsUserInAdministrationRole(userId);
            var existingSuggestion = await GetSuggestionById(suggestion.Id);

            if (existingSuggestion.UserId != userId && !hasAdministrationRole)
            {
                throw new BusinessException("No puede editar la sugerencia de otro usuario.");
            }
            
            existingSuggestion.Title = suggestion.Title;
            existingSuggestion.CategoryId = suggestion.CategoryId;
            existingSuggestion.Description = suggestion.Description;
            existingSuggestion.Approved = hasAdministrationRole ? true : null;

            _context.Update(existingSuggestion);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> DeleteSuggestion(int id, string userId)
        {
            var hasAdministrationRole = await _userService.IsUserInAdministrationRole(userId);
            var suggestion = await GetSuggestionById(id);

            if (suggestion.UserId != userId && !hasAdministrationRole)
            {
                throw new BusinessException("No puede borrar la sugerencia de otro usuario.");
            }

            _context.Remove(suggestion);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> ChangeSuggestionApprovalStatus(int id, bool? approved)
        {
            var suggestion = await GetSuggestionById(id);
            suggestion.Approved = approved;

            _context.Update(suggestion);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> ChangeSuggestionUpvotesAmount(int id, bool isIncreased)
        {
            var suggestion = await GetSuggestionById(id);

            if (isIncreased)
            {
                suggestion.UpvotesAmount++;
            }
            else
            {
                suggestion.UpvotesAmount--;
            }

            _context.Update(suggestion);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> SetSuggestionStatus(int id, int stateId)
        {
            var suggestion = await GetSuggestionById(id);
            suggestion.StateId = stateId;

            _context.Update(suggestion);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        #region HelperMethods

        private IQueryable<Suggestion> GetAllSuggestionsQuery()
        {
            var suggestionsQuery = _context.Suggestions
                                        .Include(p => p.Category)
                                        .Include(p => p.State);

            return suggestionsQuery;
        }

        private void ValidateSuggestionFields(Suggestion suggestion)
        {
            if(suggestion is null)
            {
                throw new LogicException("No existe una sugerencia para agregar o editar.");
            }

            if (string.IsNullOrEmpty(suggestion.Title) || suggestion.CategoryId == 0)
            {
                throw new BusinessException("Debe agregar todos los campos requeridos.");
            }

            if (suggestion.Title.Length > 100 || suggestion.Description?.Length > 1000)
            {
                throw new BusinessException("El título debe tener 100 caracteres o menos y la descripción debe tener 1000 caracteres o menos.");
            }
        }
        
        private async Task ValidateCaptchaField(string captchaToken)
        {
            var result = await CaptchaService.ValidateCaptchaToken(captchaToken);

            if (!result)
            {
                throw new BusinessException("El captcha ingresado no es válido.");
            }
        }

        #endregion
    }
}