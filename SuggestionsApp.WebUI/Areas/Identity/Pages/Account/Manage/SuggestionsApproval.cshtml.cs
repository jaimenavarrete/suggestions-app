using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuggestionsApp.Models.Interfaces;
using SuggestionsApp.WebUI.ViewModels;

namespace SuggestionsApp.WebUI.Areas.Identity.Pages.Account.Manage
{
    public class SuggestionsApprovalModel : PageModel
    {
        private readonly ISuggestionsService _suggestionsService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public SuggestionsApprovalModel(
            ISuggestionsService suggestionsService,
            IMapper mapper,
            IUserService userService)
        {
            _suggestionsService = suggestionsService;
            _mapper = mapper;
            _userService = userService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IEnumerable<PendingSuggestionViewModel> PendingSuggestionsList { get; set; }

        public int PendingSuggestionsCount { get; set; }

        [BindProperty]
        public ApproveSuggestionForm Input { get; set; }

        public class ApproveSuggestionForm
        {
            public int SuggestionId { get; set; }

            public bool Approved { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var suggestions = await _suggestionsService.GetSuggestions(isApproved: null);

            PendingSuggestionsList = _mapper.Map<IEnumerable<PendingSuggestionViewModel>>(suggestions);
            PendingSuggestionsCount = PendingSuggestionsList.Count();

            foreach (var suggestion in PendingSuggestionsList)
            {
                var user = suggestions.First(s => s.Id == suggestion.Id);
                suggestion.UserName = await _userService.GetUserNameById(user.UserId);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var suggestion = await _suggestionsService.GetSuggestionById(Input.SuggestionId);

            if (suggestion is null)
                RedirectToPage();

            suggestion.Approved = Input.Approved;

            var succeeded = await _suggestionsService.UpdateSuggestion(suggestion);

            if (succeeded && Input.Approved)
                StatusMessage = "La sugerencia se ha aprobado correctamente.";
            else if (succeeded && !Input.Approved)
                StatusMessage = "La sugerencia se ha rechazado correctamente.";

            return RedirectToPage();
        }
    }
}