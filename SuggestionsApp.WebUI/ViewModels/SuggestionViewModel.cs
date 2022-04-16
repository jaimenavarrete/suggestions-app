namespace SuggestionsApp.WebUI.ViewModels
{
    public class SuggestionViewModel
    {
        public int? Id { get; set; }

        public string Title { get; set; } = null!;

        public DateTime Date { get; set; }

        public int UpvotesAmount { get; set; }

        public bool IsUserUpvoteActive { get; set; }

        public string CategoryName { get; set; } = null!;

        public string StateName { get; set; } = null!;

        public string StateColorHexCode { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public bool IsUserSuggestion { get; set; }
        
        public bool IsAdminOrModeratorUser { get; set; }
    }
}
