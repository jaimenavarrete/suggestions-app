namespace SuggestionsApp.WebUI.ViewModels
{
    public class StatesListViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ColorHexCode { get; set; } = null!;

        public List<StateViewModel> StatesList { get; set; } = null!;
    }
}
