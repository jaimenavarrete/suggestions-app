using Microsoft.AspNetCore.Mvc;

namespace SuggestionsApp.WebUI.ViewModels
{
    public class SuggestionFormViewModel
    {
        public int? Id { get; set; }

        public string Title { get; set; } = null!;

        public int CategoryId { get; set; }

        public string? Description { get; set; }

        [BindProperty(Name = "g-recaptcha-response")]
        public string CaptchaToken { get; set; } = null!;

        public List<CategoryViewModel> Categories { get; set; } = null!;
    }
}
