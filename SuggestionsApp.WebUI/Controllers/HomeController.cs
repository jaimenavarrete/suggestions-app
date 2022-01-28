using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuggestionsApp.Models.Data.Identity;
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

        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            ISuggestionsService suggestionsService, 
            ICategoriesService categoriesService,
            IStatesService statesService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _suggestionsService = suggestionsService;
            _categoriesService = categoriesService;
            _statesService = statesService;

            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var suggestions = await _suggestionsService.GetSuggestions();
            var categories = await _categoriesService.GetCategories();
            var states = await _statesService.GetStates();

            var suggestionsViewModel = _mapper.Map<List<SuggestionViewModel>>(suggestions);
            var categoriesViewModel = _mapper.Map<List<CategoryViewModel>>(categories);
            var statesViewModel = _mapper.Map<List<StateViewModel>>(states);

            var indexModel = new IndexViewModel
            {
                SuggestionsList = suggestionsViewModel,
                CategoriesList = categoriesViewModel,
                StatesList = statesViewModel
            };

            return View(indexModel);
        }

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