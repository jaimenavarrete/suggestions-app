using Microsoft.AspNetCore.Identity;
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

        public async Task<IEnumerable<Suggestion>> GetSuggestions()
        {
            var suggestions = await _context.Suggestions.ToListAsync();

            return suggestions;
        }

        public async Task<Suggestion> GetSuggestionById(int id)
        {
            var suggestion = await _context.Suggestions.FindAsync(id);

            return suggestion;
        }

        public async Task<Suggestion> InsertSuggestion(Suggestion suggestion)
        {
            _context.Add(suggestion);
            await _context.SaveChangesAsync();

            return suggestion;
        }

        public async Task<Suggestion> UpdateSuggestion(Suggestion suggestion)
        {
            _context.Update(suggestion);
            await _context.SaveChangesAsync();

            return suggestion;
        }

        public Task<bool> DeleteSuggestion(int id)
        {
            throw new NotImplementedException();
        }        
    }
}