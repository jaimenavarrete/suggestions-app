using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuggestionsApp.Models.Data.Identity;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.Interfaces;
using SuggestionsApp.Models.ViewModels;
using System.Diagnostics;

namespace SuggestionsApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISuggestionsService _suggestionsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IStatesService _statesService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public HomeController(
            ISuggestionsService suggestionsService, 
            ICategoriesService categoriesService,
            IStatesService statesService,
            IMapper mapper,
            IUserService userService)
        {
            _suggestionsService = suggestionsService;
            _categoriesService = categoriesService;
            _statesService = statesService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId, int? stateId, string? search)
        {
            var suggestions = await _suggestionsService.GetSearchedSuggestions(isApproved: true, categoryId, stateId, search);
            var categories = await _categoriesService.GetCategories();
            var states = await _statesService.GetStates();

            var suggestionsViewModel = _mapper.Map<List<SuggestionViewModel>>(suggestions);
            var categoriesViewModel = _mapper.Map<List<CategoryViewModel>>(categories);
            var statesViewModel = _mapper.Map<List<StateViewModel>>(states);

            foreach(var suggestion in suggestionsViewModel)
            {
                var user = suggestions.First(s => s.Id == suggestion.Id);
                suggestion.UserName = await _userService.GetUserNameById(user.UserId);
            }

            IndexViewModel viewModel = new()
            {
                SuggestionsAmount = suggestionsViewModel.Count(),
                SuggestionsList = suggestionsViewModel,
                CategoriesList = categoriesViewModel,
                StatesList = statesViewModel
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
    }
}