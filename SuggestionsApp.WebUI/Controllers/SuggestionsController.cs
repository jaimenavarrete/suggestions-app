using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.Exceptions;
using SuggestionsApp.Models.Interfaces;
using SuggestionsApp.WebUI.ViewModels;

namespace SuggestionsApp.WebUI.Controllers
{
    [Authorize]
    public class SuggestionsController : Controller
    {
        private readonly ISuggestionsService _suggestionsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IStatesService _statesService;
        private readonly IUserService _userService;
        private readonly IUpvotesService _upvotesService;
        private readonly IMapper _mapper;

        public SuggestionsController(
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ViewSuggestion(int id)
        {
            try
            {
                var suggestion = await _suggestionsService.GetSuggestionById(id);
                var currentUserId = await _userService.GetLoggedUserId(User);
                var currentUserRole = await _userService.GetUserRoleById(currentUserId);
                var currentUserUpvote = await _upvotesService.GetSuggestionUserUpvote(suggestion.Id, currentUserId);

                var viewModel = _mapper.Map<ViewSuggestionViewModel>(suggestion);
                viewModel.UserName = await _userService.GetUserNameById(suggestion.UserId);
                viewModel.IsAdminOrModeratorUser = currentUserRole is "Admin" or "Moderador";
                viewModel.IsUserSuggestion = suggestion.UserId == currentUserId;
                viewModel.IsUserUpvoteActive = currentUserUpvote is not null;
                viewModel.States = await GetStatesViewModel();

                return View(viewModel);
            }
            catch (LogicException ex)
            {
                TempData["error"] = ex.Message;

                return RedirectToAction("Index", "Home");
            }
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
            try
            {
                var suggestion = _mapper.Map<Suggestion>(viewModel);
                suggestion.UserId = await _userService.GetLoggedUserId(User);
                suggestion.UpvotesAmount = 0;
                suggestion.Date = DateTime.Now;

                var succeeded = await _suggestionsService.InsertSuggestion(suggestion);

                if (succeeded)
                    TempData["success"] = "La sugerencia se ha creado correctamente.";

                return RedirectToAction("Index", "Home");
            }
            catch(LogicException ex)
            {
                TempData["error"] = ex.Message;

                return RedirectToAction();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditSuggestion(int id)
        {
            var suggestion = await _suggestionsService.GetSuggestionById(id);

            SuggestionFormViewModel viewModel = new()
            {
                Id = id,
                Categories = await GetCategoriesViewModel(),
                Title = suggestion.Title.Trim(),
                CategoryId = suggestion.CategoryId,
                Description = suggestion.Description!,
            };

            return View("CreateSuggestion", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditSuggestion(SuggestionFormViewModel viewModel)
        {
            try
            {
                var suggestion = await _suggestionsService.GetSuggestionById(viewModel.Id ?? 0);

                suggestion.Title = viewModel.Title;
                suggestion.CategoryId = viewModel.CategoryId;
                suggestion.Description = viewModel.Description;

                var succeeded = await _suggestionsService.UpdateSuggestion(suggestion);

                if (succeeded)
                    TempData["success"] = "La sugerencia se ha editado correctamente.";

                return RedirectToAction("ViewSuggestion", new { id = viewModel.Id });
            }
            catch (LogicException ex)
            {
                TempData["error"] = ex.Message;

                return RedirectToAction();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetSuggestionStatus([FromForm] int suggestionId, int stateId)
        {
            var suggestion = await _suggestionsService.GetSuggestionById(suggestionId);

            suggestion.StateId = stateId;

            var succeeded = await _suggestionsService.UpdateSuggestion(suggestion);

            if (succeeded)
                TempData["success"] = "El estado de la sugerencia se ha cambiado correctamente.";

            return RedirectToAction("ViewSuggestion", new { id = suggestionId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSuggestion([FromForm] int suggestionId)
        {
            try
            {
                var succeeded = await _suggestionsService.DeleteSuggestion(suggestionId);

                if (succeeded)
                    TempData["success"] = "La sugerencia se ha borrado correctamente.";

                return RedirectToAction("Index", "Home");
            }
            catch(LogicException ex)
            {
                TempData["error"] = ex.Message;

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSuggestionUpvote([FromForm] int suggestionId, bool isFromViewSuggestion)
        {
            try
            {
                var userId = await _userService.GetLoggedUserId(User);
                var currentUserUpvote = await _upvotesService.GetSuggestionUserUpvote(suggestionId, userId);

                if(currentUserUpvote is not null)
                {
                    var succeeded = await _upvotesService.DeleteUpvote(suggestionId, userId);
                }
                else
                {
                    var upvote = new Upvote(suggestionId, userId);
                    await _upvotesService.InsertUpvote(upvote);
                }

                return GetUpvoteRedirect(suggestionId, isFromViewSuggestion);
            }
            catch (LogicException ex)
            {
                TempData["error"] = ex.Message;

                return GetUpvoteRedirect(suggestionId, isFromViewSuggestion);
            }
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

            private RedirectToActionResult GetUpvoteRedirect(int id, bool isFromViewSuggestion)
            {
                if (isFromViewSuggestion)
                {
                    return RedirectToAction("ViewSuggestion", "Suggestions", new { id = id });
                }

                return RedirectToAction("Index", "Home");
            }

        #endregion
    }
}
