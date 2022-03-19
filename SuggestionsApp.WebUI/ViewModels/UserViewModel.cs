namespace SuggestionsApp.WebUI.ViewModels
{
    public class UserViewModel
    {
        public string? Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public string Role { get; set; } = null!;

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        public bool LockedOut { get; set; }
    }
}
