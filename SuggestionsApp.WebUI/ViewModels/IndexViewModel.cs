namespace SuggestionsApp.WebUI.ViewModels
{
    public class IndexViewModel
    {
        public string? SearchText { get; set; }

        public int? CategorySearchId { get; set; }

        public int? StateSearchId { get; set; }

        public int SuggestionsAmount { get; set; }

        public List<SuggestionViewModel> SuggestionsList { get; set; } = null!;

        public List<CategoryViewModel> CategoriesList { get; set; } = null!;

        public List<StateViewModel> StatesList { get; set; } = null!;
    }
}
