using System.Text.RegularExpressions;
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

        public async Task<State> GetStateById(int id)
        {
            var state = await _context.States.FindAsync(id);

            if (state is null)
            {
                throw new LogicException("El estado que seleccionó no existe.");
            }
            
            return state;
        }

        public async Task<bool> InsertState(State state)
        {
            ValidateStateFields(state);

            _context.Add(state);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> UpdateState(State state)
        {
            ValidateStateFields(state);

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
        
        #region HelperMethods

        private void ValidateStateFields(State state)
        {
            if(state is null)
            {
                throw new LogicException("No existe un estado para crear o editar.");
            }

            if (string.IsNullOrEmpty(state.Name) || string.IsNullOrEmpty(state.Description) || string.IsNullOrEmpty(state.ColorHexCode))
            {
                throw new BusinessException("Debe agregar todos los campos requeridos.");
            }

            if (!ColorHexCodeValidation(state.ColorHexCode))
            {
                throw new BusinessException("Debe agregar un color válido.");
            }
        }

        private bool ColorHexCodeValidation(string hexCode)
        {
            var regex = new Regex(@"^#([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$");
            return regex.IsMatch(hexCode);
        }

        #endregion
    }
}
