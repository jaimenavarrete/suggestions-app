using SuggestionsApp.Models.DataModels;

namespace SuggestionsApp.Models.Interfaces
{
    public interface IStatesService
    {
        Task<IEnumerable<State>> GetStates();
        Task<State?> GetStateById(int id);
        Task<bool> InsertState(State state);
        Task<bool> UpdateState(State state);
        Task<bool> DeleteState(int id);
    }
}
