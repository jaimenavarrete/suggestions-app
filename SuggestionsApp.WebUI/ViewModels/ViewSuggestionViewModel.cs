namespace SuggestionsApp.WebUI.ViewModels
{
    public class ViewSuggestionViewModel
    {
        public int? Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int UpvotesAmount { get; set; }

        public bool IsUserUpvoteActive { get; set; }

        public string CategoryName { get; set; } = null!;

        public int? StateId { get; set; }

        public string? StateName { get; set; }

        public string? StateDescription { get; set; }

        public string? StateColorHexCode { get; set; }

        public string UserName { get; set; } = null!;

        public bool IsUserInAdministrationRole { get; set; }

        public bool IsUserSuggestion { get; set; }

        public DateTime Date { get; set; }

        public bool? Approved { get; set; }

        public List<StateViewModel> States { get; set; } = null!;
    }
}
