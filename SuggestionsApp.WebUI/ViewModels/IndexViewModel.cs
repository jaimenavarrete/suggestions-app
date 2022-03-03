namespace SuggestionsApp.WebUI.ViewModels
{
    public class IndexViewModel
    {
        public int SuggestionsAmount { get; set; }

        public List<SuggestionViewModel> SuggestionsList { get; set; } = null!;

        public List<CategoryViewModel> CategoriesList { get; set; } = null!;

        public List<StateViewModel> StatesList { get; set; } = null!;
    }
}
