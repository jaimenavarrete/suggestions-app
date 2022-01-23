namespace SuggestionsApp.Models.DataModels
{
    public partial class State
    {
        public State()
        {
            Suggestions = new HashSet<Suggestion>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ColorHexCode { get; set; } = null!;

        public virtual ICollection<Suggestion> Suggestions { get; set; }
    }
}
