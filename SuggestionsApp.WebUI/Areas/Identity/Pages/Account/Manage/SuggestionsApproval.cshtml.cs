using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuggestionsApp.Models.Data.Identity;
using SuggestionsApp.Models.Interfaces;
using SuggestionsApp.Models.ViewModels;

namespace SuggestionsApp.WebUI.Areas.Identity.Pages.Account.Manage
{
    public class SuggestionsApprovalModel : PageModel
    {
        private readonly ISuggestionsService _suggestionsService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SuggestionsApprovalModel(
            ISuggestionsService suggestionsService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _suggestionsService = suggestionsService;
            _mapper = mapper;
            
            _userManager = userManager;
            _signInManager = signInManager;
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
                suggestion.UserName = await GetUserName(user.UserId);
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
            
            if(succeeded && Input.Approved)
                StatusMessage = "La sugerencia se ha aprobado correctamente.";
            else if(succeeded && !Input.Approved)
                StatusMessage = "La sugerencia se ha rechazado correctamente.";

            return RedirectToPage();
        }
        
        #region HelperMethods

        private async Task<string> GetUserName(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                throw new Exception("El usuario no existe");

            return user.UserName;
        }

        #endregion
    }
}