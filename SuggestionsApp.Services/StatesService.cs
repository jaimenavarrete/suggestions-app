using Microsoft.EntityFrameworkCore;
using SuggestionsApp.Models.Data.Database;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.Interfaces;

namespace SuggestionsApp.Services
{
    public class StatesService : IStatesService
    {
        private readonly SuggestionsAppContext _context;

        public StatesService(SuggestionsAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<State>> GetStates()
        {
            var states = await _context.States.ToListAsync();

            return states;
        }

        public async Task<State?> GetStateById(int id)
        {
            var state = await _context.States.FindAsync(id);
            
            return state;
        }

        public Task<bool> InsertState(State state)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateState(State state)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteState(int id)
        {
            throw new NotImplementedException();
        }
    }
}
