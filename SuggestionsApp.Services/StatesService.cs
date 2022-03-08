using Microsoft.EntityFrameworkCore;
using SuggestionsApp.Models.Data.Database;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.Exceptions;
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

        public async Task<bool> InsertState(State state)
        {
            if (state is null)
            {
                throw new LogicException("No existe un estado para crear.");
            }

            _context.Add(state);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> UpdateState(State state)
        {
            if (state is null)
            {
                throw new LogicException("No existe un estado para editar.");
            }

            _context.Update(state);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> DeleteState(int id)
        {
            var state = await GetStateById(id);

            if (state is null)
            {
                throw new LogicException("No existe un estado para borrar.");
            }

            _context.States.Remove(state);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }
    }
}
