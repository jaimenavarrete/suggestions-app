namespace SuggestionsApp.Models.DataModels
{
    public partial class Upvote
    {
        public Upvote(int suggestionId, string userId)
        {
            SuggestionId = suggestionId;
            UserId = userId;
        }

        public int Id { get; set; }
        
        public int SuggestionId { get; set; }
        
        public string? UserId { get; set; }

        public virtual Suggestion Suggestion { get; set; } = null!;
    }
}
