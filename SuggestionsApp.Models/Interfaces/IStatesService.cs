using SuggestionsApp.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuggestionsApp.Models.Interfaces
{
    public interface IStatesService
    {
        Task<IEnumerable<State>> GetStates();
        Task<State> GetStateById(int id);
        Task<State> InsertState(State state);
        Task<State> UpdateState(State state);
        Task<bool> DeleteState(int id);
    }
}
