using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SuggestionsApp.Models.Interfaces;
using SuggestionsApp.WebUI.ViewModels;
using System.Diagnostics;

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
        public async Task<IActionResult> Index(int? categoryId, int? stateId, string? search)
        {
            // The categoryId, stateId and search parameters are to filter suggestions and they are optional
            var suggestionsViewModel = await GetApprovedSuggestionsViewModel(categoryId, stateId, search);

            IndexViewModel viewModel = new()
            {
                SearchText = search,
                CategorySearchId = categoryId,
                StateSearchId = stateId,
                SuggestionsAmount = suggestionsViewModel.Count(),
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

            private async Task<List<SuggestionViewModel>> GetApprovedSuggestionsViewModel(int? categoryId, int? stateId, string? search)
            {
                var suggestions = await _suggestionsService.GetSearchedSuggestions(isApproved: true, categoryId, stateId, search);
                var suggestionsViewModel = _mapper.Map<List<SuggestionViewModel>>(suggestions);
                var currentUserId = await _userService.GetUserIdLoggedIn();

                foreach (var suggestionViewModel in suggestionsViewModel)
                {
                    var suggestion = suggestions.First(s => s.Id == suggestionViewModel.Id);
                    suggestionViewModel.UserName = await _userService.GetUserNameById(suggestion.UserId);
                    suggestionViewModel.IsUserUpvoteActive = await _upvotesService.IsSuggestionUserUpvoteActive(suggestion.Id, currentUserId);
                    suggestionViewModel.IsUserSuggestion = suggestion.UserId == currentUserId;
                }

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