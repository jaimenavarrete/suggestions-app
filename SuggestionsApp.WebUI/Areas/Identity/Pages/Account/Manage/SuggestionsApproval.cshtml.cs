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

        public IEnumerable<SuggestionViewModel> PendingSuggestionsList { get; set; }

        [BindProperty]
        public ApproveSuggestion Input { get; set; }

        public class ApproveSuggestion
        {
            public int Id { get; set; }
            
            public bool Approved { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var suggestions = await _suggestionsService.GetSuggestions(isApproved: false);
            
            PendingSuggestionsList = _mapper.Map<IEnumerable<SuggestionViewModel>>(suggestions);

            return Page();
        }
    }
}