namespace SuggestionsApp.Models.DataModels
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; } = null!;

        public string SecurityStamp { get; set; } = null!;

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }
    }
}
