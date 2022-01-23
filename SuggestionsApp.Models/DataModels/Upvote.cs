namespace SuggestionsApp.Models.DataModels
{
    public partial class Upvote
    {
        public int Id { get; set; }
        public int SuggestionId { get; set; }
        public int UserId { get; set; }

        public virtual Suggestion Suggestion { get; set; } = null!;
    }
}
