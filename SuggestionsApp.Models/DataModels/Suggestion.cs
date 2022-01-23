namespace SuggestionsApp.Models.DataModels
{
    public partial class Suggestion
    {
        public Suggestion()
        {
            Upvotes = new HashSet<Upvote>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int UpvotesAmount { get; set; }
        public int CategoryId { get; set; }
        public int? StateId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public bool Approved { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual State? State { get; set; }
        public virtual ICollection<Upvote> Upvotes { get; set; }
    }
}
