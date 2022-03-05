namespace SuggestionsApp.WebUI.ViewModels
{
    public class SuggestionsApprovalViewModel
    {
        public int SuggestionId { get; set; }

        public bool Approved { get; set; }

        public IEnumerable<PendingSuggestionViewModel> PendingSuggestionsList { get; set; } = null!;

        public int PendingSuggestionsCount { get; set; }
    }
}
