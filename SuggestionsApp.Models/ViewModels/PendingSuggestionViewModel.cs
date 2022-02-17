namespace SuggestionsApp.Models.ViewModels
{
    public class PendingSuggestionViewModel
    {
        public int Id { get; set; }
        
        public string Title { get; set; } = null!;
        
        public string? Description { get; set; }

        public string CategoryName { get; set; } = null!;

        public string UserName { get; set; } = null!;
        
        public DateTime Date { get; set; }
    }
}