namespace SuggestionsApp.Models.ViewModels
{
    public class SuggestionFormViewModel
    {
        public int? Id { get; set; }

        public string Title { get; set; } = null!;

        public List<CategoryViewModel> Categories { get; set; } = null!;

        public int CategoryId { get; set; }

        public string Description { get; set; } = null!;
    }
}
