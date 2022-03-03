namespace SuggestionsApp.WebUI.ViewModels
{
    public class SuggestionViewModel
    {
        public int? Id { get; set; }

        public string Title { get; set; } = null!;

        public int UpvotesAmount { get; set; }

        public string CategoryName { get; set; } = null!;

        public string StateName { get; set; } = null!;

        public string StateColorHexCode { get; set; } = null!;

        public string UserName { get; set; } = null!;
    }
}
