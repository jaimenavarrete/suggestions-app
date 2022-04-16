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
                var currentUserUpvote = await _upvotesService.GetSuggestionUserUpvote(suggestion.Id, currentUserId);

                var viewModel = _mapper.Map<ViewSuggestionViewModel>(suggestion);
                viewModel.UserName = await _userService.GetUserNameById(suggestion.UserId);
                viewModel.IsAdminOrModeratorUser = await _userService.HasAdministrationRole(currentUserId);
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
            var categoriesViewModel = await GetCategoriesViewModel();

            if (categoriesViewModel.Count == 0)
            {
                TempData["error"] = "No se pueden crear sugerencias si no hay categorías disponibles.";
                return RedirectToAction("Index", "Home");
            }

            SuggestionFormViewModel viewModel = new()
            {
                Categories = categoriesViewModel,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSuggestion(SuggestionFormViewModel viewModel)
        {
            try
            {
                var currentUserId = await _userService.GetLoggedUserId(User);
                var suggestion = _mapper.Map<Suggestion>(viewModel);

                var succeeded = await _suggestionsService.InsertSuggestion(suggestion, currentUserId, viewModel.CaptchaToken);

                if (succeeded)
                    TempData["success"] = "La sugerencia se ha creado correctamente.";

                return RedirectToAction("Index", "Home");
            }
            catch(BusinessException ex)
            {
                TempData["error"] = ex.Message;

                return RedirectToAction();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditSuggestion(int id)
        {
            var currentUserId = await _userService.GetLoggedUserId(User);
            var hasAdministrationRole = await _userService.HasAdministrationRole(currentUserId);
            var suggestion = await _suggestionsService.GetSuggestionById(id);

            if (currentUserId != suggestion.UserId && !hasAdministrationRole)
            {
                TempData["error"] = "No puede editar la sugerencia de otro usuario.";
                return RedirectToAction("Index", "Home");
            }

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
                var currentUserId = await _userService.GetLoggedUserId(User);
                var suggestion = _mapper.Map<Suggestion>(viewModel);

                var succeeded = await _suggestionsService.UpdateSuggestion(suggestion, currentUserId);

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
        [Authorize(Roles = "Admin,Moderador")]
        public async Task<IActionResult> SetSuggestionStatus([FromForm] int suggestionId, int stateId)
        {
            var succeeded = await _suggestionsService.SetSuggestionStatus(suggestionId, stateId);

            if (succeeded)
                TempData["success"] = "El estado de la sugerencia se ha cambiado correctamente.";

            return RedirectToAction("ViewSuggestion", new { id = suggestionId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSuggestion([FromForm] int suggestionId)
        {
            try
            {
                var currentUserId = await _userService.GetLoggedUserId(User);
                var succeeded = await _suggestionsService.DeleteSuggestion(suggestionId, currentUserId);

                if (succeeded)
                    TempData["success"] = "La sugerencia se ha borrado correctamente.";

                return RedirectToAction("Index", "Home");
            }
            catch(BusinessException ex)
            {
                TempData["error"] = ex.Message;

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSuggestionUpvote([FromForm] int suggestionId, bool isFromViewSuggestionPage)
        {
            try
            {
                var currentUserId = await _userService.GetLoggedUserId(User);
                var currentUserUpvote = await _upvotesService.GetSuggestionUserUpvote(suggestionId, currentUserId);

                if(currentUserUpvote is not null)
                {
                    await _upvotesService.DeleteUpvote(currentUserUpvote);
                }
                else
                {
                    var upvote = new Upvote(suggestionId, currentUserId);
                    await _upvotesService.InsertUpvote(upvote);
                }

                return GetUpvoteRedirect(suggestionId, isFromViewSuggestionPage);
            }
            catch (LogicException ex)
            {
                TempData["error"] = ex.Message;

                return GetUpvoteRedirect(suggestionId, isFromViewSuggestionPage);
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
                    return RedirectToAction("ViewSuggestion", "Suggestions", new { id });
                }

                return RedirectToAction("Index", "Home");
            }

        #endregion
    }
}
