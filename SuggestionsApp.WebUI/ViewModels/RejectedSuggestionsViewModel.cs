namespace SuggestionsApp.WebUI.ViewModels;

public class RejectedSuggestionsViewModel
{
    public int SuggestionId { get; set; }

    public IEnumerable<PendingSuggestionViewModel> RejectedSuggestionsList { get; set; } = null!;

    public int RejectedSuggestionsCount { get; set; }
}