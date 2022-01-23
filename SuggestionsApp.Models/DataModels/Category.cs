namespace SuggestionsApp.Models.DataModels
{
    public partial class Category
    {
        public Category()
        {
            Suggestions = new HashSet<Suggestion>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Suggestion> Suggestions { get; set; }
    }
}
