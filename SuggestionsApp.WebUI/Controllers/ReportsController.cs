using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Rotativa.AspNetCore;
using SuggestionsApp.Models.Interfaces;
using SuggestionsApp.WebUI.ViewModels;

namespace SuggestionsApp.WebUI.Controllers
{
    [Authorize(Roles = "Admin,Moderador")]
    public class ReportsController : Controller
    {
        private readonly ISuggestionsService _suggestionsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IStatesService _statesService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ReportsController(
            ISuggestionsService suggestionsService,
            ICategoriesService categoriesService,
            IStatesService statesService,
            IUserService userService,
            IMapper mapper)
        {
            _suggestionsService = suggestionsService;
            _categoriesService = categoriesService;
            _statesService = statesService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> PrintApprovedSuggestions()
        {
            var approvedSuggestionsViewModel = await GetSuggestionsViewModel(isApproved: true);
            return CreatePdf("PrintSuggestions", approvedSuggestionsViewModel, "Reporte de sugerencias aprobadas");
        }
        
        [HttpGet]
        public async Task<IActionResult> PrintPendingSuggestions()
        {
            var pendingSuggestionsViewModel = await GetSuggestionsViewModel(isApproved: null);
            return CreatePdf("PrintSuggestions", pendingSuggestionsViewModel, "Reporte de sugerencias pendientes");
        }

        [HttpGet]
        public async Task<IActionResult> PrintCategories()
        {
            var categories = await _categoriesService.GetCategories();
            var categoriesViewModel = _mapper.Map<List<CategoryViewModel>>(categories);
            return CreatePdf(categoriesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PrintStates()
        {
            var states = await _statesService.GetStates();
            var statesViewModel = _mapper.Map<List<StateViewModel>>(states);
            return CreatePdf(statesViewModel);
        }
        
        [HttpGet]
        public async Task<IActionResult> PrintUsers()
        {
            var users = await _userService.GetUsers();
            var usersViewModel = _mapper.Map<List<UserViewModel>>(users);

            foreach (var user in usersViewModel)
            {
                user.LockedOut = user.LockoutEnd > DateTime.UtcNow;
                user.Role = await _userService.GetUserRoleById(user.Id ?? "");
            }
            
            return CreatePdf(usersViewModel);
        }

        #region HelperMethods

        private async Task<List<PendingSuggestionViewModel>> GetSuggestionsViewModel(bool? isApproved)
        {
            var suggestions = await _suggestionsService.GetSuggestions(isApproved);
            var suggestionsViewModel = _mapper.Map<List<PendingSuggestionViewModel>>(suggestions);

            foreach (var suggestion in suggestionsViewModel)
            {
                suggestion.UserName = await _userService.GetUserNameById(suggestion.UserId);
                suggestion.Description = suggestion.Description?.Length > 250 ? $"{suggestion.Description[..250]}..." : suggestion.Description;
            }

            return suggestionsViewModel;
        }

        private ViewAsPdf CreatePdf(object list)
        {
            return new ViewAsPdf(list)
            {
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 10"
            };
        }
        
        private ViewAsPdf CreatePdf(string viewName, object list, string title)
        {
            return new ViewAsPdf(viewName, list, new ViewDataDictionary(ViewData) { { "Title", title } })
            {
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 10"
            };
        }

        #endregion
    }
}
