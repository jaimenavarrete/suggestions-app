using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SuggestionsApp.Models.Interfaces;
using SuggestionsApp.WebUI.ViewModels;

namespace SuggestionsApp.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISuggestionsService _suggestionsService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AdminController(
            ISuggestionsService suggestionsService,
            IMapper mapper,
            IUserService userService
            )
        {
            _suggestionsService = suggestionsService;
            _userService = userService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CategoriesList()
        {
            return View();
        }

        public IActionResult StatesList()
        {
            return View();
        }

        public IActionResult UsersList()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SuggestionsApproval()
        {
            var pendingSuggestions = await _suggestionsService.GetSuggestions(isApproved: null);

            var pendingSuggestionsViewModelList = _mapper.Map<IEnumerable<PendingSuggestionViewModel>>(pendingSuggestions);
            var pendingSuggestionsCount = pendingSuggestionsViewModelList.Count();

            foreach (var suggestion in pendingSuggestionsViewModelList)
            {
                var user = pendingSuggestions.First(s => s.Id == suggestion.Id);
                suggestion.UserName = await _userService.GetUserNameById(user.UserId);
            }

            SuggestionsApprovalViewModel viewModel = new()
            {
                PendingSuggestionsList = pendingSuggestionsViewModelList,
                PendingSuggestionsCount = pendingSuggestionsCount
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SuggestionsApproval(SuggestionsApprovalViewModel viewModel)
        {
            var suggestion = await _suggestionsService.GetSuggestionById(viewModel.SuggestionId);

            suggestion.Approved = viewModel.Approved;

            var succeeded = await _suggestionsService.UpdateSuggestion(suggestion);

            if (succeeded && viewModel.Approved)
                TempData["success"] = "La sugerencia se ha aprobado correctamente.";
            else if (succeeded && !viewModel.Approved)
                TempData["success"] = "La sugerencia se ha rechazado correctamente.";
            else
                TempData["error"] = "Ocurrió un inconveniente al aprobar la sugerencia";

            return RedirectToAction();
        }
    }
}
