using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SuggestionsApp.Models.Interfaces;
using SuggestionsApp.WebUI.ViewModels;
using System.Diagnostics;
using SuggestionsApp.Models.QueryFilters;

namespace SuggestionsApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISuggestionsService _suggestionsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IStatesService _statesService;
        private readonly IUserService _userService;
        private readonly IUpvotesService _upvotesService;
        private readonly IMapper _mapper;

        public HomeController(
            ISuggestionsService suggestionsService, 
            ICategoriesService categoriesService,
            IStatesService statesService,
            IUserService userService,
            IUpvotesService upvotesService,
            IMapper mapper)
        {
            _suggestionsService = suggestionsService;
            _categoriesService = categoriesService;
            _statesService = statesService;
            _userService = userService;
            _upvotesService = upvotesService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(SuggestionQueryFilter filters)
        {
            var currentUserId = await _userService.GetLoggedUserId(User);
            var suggestionsViewModel = await GetSuggestionsViewModel(filters, currentUserId);

            foreach (var suggestion in suggestionsViewModel)
            {
                suggestion.UserName = await _userService.GetUserNameById(suggestion.UserId);
                suggestion.IsUserUpvoteActive = await _upvotesService.SuggestionHasUserUpvote(suggestion.Id ?? 0, currentUserId);
                suggestion.IsUserSuggestion = suggestion.UserId == currentUserId;
                suggestion.IsUserInAdministrationRole = await _userService.IsUserInAdministrationRole(currentUserId);
            }

            IndexViewModel viewModel = new()
            {
                SearchText = filters.SearchText,
                CategorySearchId = filters.CategoryId,
                StateSearchId = filters.StateId,
                OrderBy = filters.OrderBy,
                SuggestionsAmount = suggestionsViewModel.Count,
                SuggestionsList = suggestionsViewModel,
                CategoriesList = await GetCategoriesViewModel(),
                StatesList = await GetStatesViewModel()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region HelperMethods
        
        private async Task<List<SuggestionViewModel>> GetSuggestionsViewModel(SuggestionQueryFilter filters, string userId)
        {
            filters.UserId = userId;
            var suggestions = await _suggestionsService.GetSearchedSuggestions(isApproved: true, filters);
            var suggestionsViewModel = _mapper.Map<List<SuggestionViewModel>>(suggestions);

            return suggestionsViewModel;
        }

        private async Task<List<CategoryViewModel>> GetCategoriesViewModel()
        {
            var categories = await _categoriesService.GetCategories();
            var categoriesViewModel = _mapper.Map<List<CategoryViewModel>>(categories);

            return categoriesViewModel;
        }

        private async Task<List<StateViewModel>> GetStatesViewModel()
        {
            var states = await _statesService.GetStates();
            var statesViewModel = _mapper.Map<List<StateViewModel>>(states);

            return statesViewModel;
        }

        #endregion
    }
}