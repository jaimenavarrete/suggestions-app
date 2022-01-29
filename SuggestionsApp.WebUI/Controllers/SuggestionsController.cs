using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuggestionsApp.Models.Data.Identity;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.Interfaces;
using SuggestionsApp.Models.ViewModels;

namespace SuggestionsApp.WebUI.Controllers
{
    public class SuggestionsController : Controller
    {
        private readonly ISuggestionsService _suggestionsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public SuggestionsController(
            ISuggestionsService suggestionsService,
            ICategoriesService categoriesService, 
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _suggestionsService = suggestionsService;
            _categoriesService = categoriesService;

            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateSuggestion()
        {
            SuggestionFormViewModel viewModel = new()
            {
                Categories = await GetCategoriesViewModel(),
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSuggestion(SuggestionFormViewModel viewModel)
        {
            var suggestion = _mapper.Map<Suggestion>(viewModel);
            suggestion.UpvotesAmount = 0;
            suggestion.Approved = false;
            suggestion.Date = DateTime.UtcNow;
            suggestion.UserId = _userManager.GetUserId(User);

            var result = await _suggestionsService.InsertSuggestion(suggestion);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditSuggestion(int id)
        {
            var suggestion = await _suggestionsService.GetSuggestionById(id);

            if (suggestion == null)
                return NotFound();

            SuggestionFormViewModel viewModel = new()
            {
                Id = id,
                Categories = await GetCategoriesViewModel(),
                Title = suggestion.Title,
                CategoryId = suggestion.CategoryId,
                Description = suggestion.Description!,
            };

            return View("CreateSuggestion", viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditSuggestion(SuggestionFormViewModel viewModel)
        {
            var suggestion = await _suggestionsService.GetSuggestionById(viewModel.Id ?? 0);

            if (suggestion == null)
                return NotFound();

            suggestion.Title = viewModel.Title;
            suggestion.CategoryId = viewModel.CategoryId;
            suggestion.Description = viewModel.Description;

            var result = await _suggestionsService.UpdateSuggestion(suggestion);

            return RedirectToAction("Index", "Home");
        }

        #region HelperMethods

        private async Task<List<CategoryViewModel>> GetCategoriesViewModel()
        {
            var categories = await _categoriesService.GetCategories();
            var categoriesViewModel = _mapper.Map<List<CategoryViewModel>>(categories);

            return categoriesViewModel;
        }

        #endregion
    }
}
