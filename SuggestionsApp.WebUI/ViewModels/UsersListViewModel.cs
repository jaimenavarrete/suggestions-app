namespace SuggestionsApp.WebUI.ViewModels
{
    public class UsersListViewModel
    {
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string RepeatPassword { get; set; } = null!;

        public List<UserViewModel> UsersList { get; set; } = null!;

        public List<RoleViewModel> Roles { get; set; } = null!;
    }
}
