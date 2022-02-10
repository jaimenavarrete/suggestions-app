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
    [Authorize]
    public class SuggestionsController : Controller
    {
        private readonly ISuggestionsService _suggestionsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IStatesService _statesService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public SuggestionsController(
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ViewSuggestion(int id)
        {
            var suggestion = await _suggestionsService.GetSuggestionById(id);

            if (suggestion == null)
                return NotFound();

            var viewModel = _mapper.Map<ViewSuggestionViewModel>(suggestion);
            viewModel.UserName = await GetUserName(suggestion.UserId);
            viewModel.States = await GetStatesViewModel();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSuggestion()
        {
            SuggestionFormViewModel viewModel = new()
            {
                Categories = await GetCategoriesViewModel(),
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSuggestion(SuggestionFormViewModel viewModel)
        {
            var suggestion = _mapper.Map<Suggestion>(viewModel);
            suggestion.UpvotesAmount = 0;
            suggestion.Approved = false;
            suggestion.Date = DateTime.Now;
            suggestion.UserId = _userManager.GetUserId(User);

            var succeeded = await _suggestionsService.InsertSuggestion(suggestion);

            if(succeeded)
                TempData["success"] = "La sugerencia se ha creado correctamente.";

            return RedirectToAction("Index", "Home");
        }

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

        [HttpPost]
        public async Task<IActionResult> EditSuggestion(SuggestionFormViewModel viewModel)
        {
            var suggestion = await _suggestionsService.GetSuggestionById(viewModel.Id ?? 0);

            if (suggestion == null)
                return NotFound();

            suggestion.Title = viewModel.Title;
            suggestion.CategoryId = viewModel.CategoryId;
            suggestion.Description = viewModel.Description;

            var succeeded = await _suggestionsService.UpdateSuggestion(suggestion);

            if (succeeded)
                TempData["success"] = "La sugerencia se ha editado correctamente.";

            return RedirectToAction("ViewSuggestion", new { id = viewModel.Id});
        }

        [HttpPost]
        public async Task<IActionResult> SetSuggestionStatus([FromForm] int suggestionId, [FromForm] int stateId)
        {
            var suggestion = await _suggestionsService.GetSuggestionById(suggestionId);
            suggestion.StateId = stateId;

            var succeeded = await _suggestionsService.UpdateSuggestion(suggestion);

            if (succeeded)
                TempData["success"] = "El estado de la sugerencia se ha cambiado correctamente.";

            return RedirectToAction("ViewSuggestion", new { id = suggestionId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSuggestion([FromForm] int id)
        {
            var succeeded = await _suggestionsService.DeleteSuggestion(id);

            if (succeeded)
                TempData["success"] = "La sugerencia se ha borrado correctamente.";

            return RedirectToAction("Index", "Home");
        }

        #region HelperMethods

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

            private async Task<string> GetUserName(string userId)
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                    throw new Exception("El usuario no existe");

                return user.UserName;
            }

        #endregion
    }
}
