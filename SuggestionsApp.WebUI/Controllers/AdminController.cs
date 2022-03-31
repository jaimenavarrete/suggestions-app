using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SuggestionsApp.Models.Data.Identity;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.Exceptions;
using SuggestionsApp.Models.Interfaces;
using SuggestionsApp.WebUI.ViewModels;

namespace SuggestionsApp.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISuggestionsService _suggestionsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IStatesService _statesService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AdminController(
            ISuggestionsService suggestionsService,
            ICategoriesService categoriesService,
            IStatesService statesService,
            IMapper mapper,
            IUserService userService
            )
        {
            _suggestionsService = suggestionsService;
            _categoriesService = categoriesService;
            _statesService = statesService;
            _userService = userService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }


        // Categories section


        [HttpGet]
        public async Task<IActionResult> CategoriesList()
        {
            var categories = await _categoriesService.GetCategories();
            var categoriesViewModel = _mapper.Map<List<CategoryViewModel>>(categories);

            CategoriesListViewModel viewModel = new()
            {
                CategoriesList = categoriesViewModel
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoriesListViewModel viewModel)
        {
            try
            {
                var category = _mapper.Map<Category>(viewModel);
                var succeeded = await _categoriesService.InsertCategory(category);

                if (succeeded)
                    TempData["success"] = "La categoría se ha creado correctamente.";

                return RedirectToAction("CategoriesList");
            }
            catch (LogicException ex)
            {
                TempData["error"] = ex.Message;

                return RedirectToAction("CategoriesList");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(CategoriesListViewModel viewModel)
        {
            try
            {
                var category = _mapper.Map<Category>(viewModel);
                var succeeded = await _categoriesService.UpdateCategory(category);

                if (succeeded)
                    TempData["success"] = "La categoría se ha editado correctamente.";

                return RedirectToAction("CategoriesList");
            }
            catch (LogicException ex)
            {
                TempData["error"] = ex.Message;

                return RedirectToAction("CategoriesList");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory([FromForm] int id)
        {
            try
            {
                var succeeded = await _categoriesService.DeleteCategory(id);

                if (succeeded)
                    TempData["success"] = "La categoría se ha borrado correctamente.";

                return RedirectToAction("CategoriesList");
            }
            catch (LogicException ex)
            {
                TempData["error"] = ex.Message;

                return RedirectToAction("CategoriesList");
            }
        }


        // States section


        public async Task<IActionResult> StatesList()
        {
            var states = await _statesService.GetStates();
            var statesViewModel = _mapper.Map<List<StateViewModel>>(states);

            StatesListViewModel viewModel = new()
            {
                StatesList = statesViewModel
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateState(StatesListViewModel viewModel)
        {
            try
            {
                var state = _mapper.Map<State>(viewModel);
                var succeeded = await _statesService.InsertState(state);

                if (succeeded)
                    TempData["success"] = "El estado se ha creado correctamente.";

                return RedirectToAction("StatesList");
            }
            catch (LogicException ex)
            {
                TempData["error"] = ex.Message;

                return RedirectToAction("StatesList");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditState(StatesListViewModel viewModel)
        {
            try
            {
                var state = _mapper.Map<State>(viewModel);
                var succeeded = await _statesService.UpdateState(state);

                if (succeeded)
                    TempData["success"] = "El estado se ha editado correctamente.";

                return RedirectToAction("StatesList");
            }
            catch (LogicException ex)
            {
                TempData["error"] = ex.Message;

                return RedirectToAction("StatesList");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteState([FromForm] int id)
        {
            try
            {
                var succeeded = await _statesService.DeleteState(id);

                if (succeeded)
                    TempData["success"] = "El estado se ha borrado correctamente.";
                else
                    TempData["error"] = "Ocurrió un error al borrar el estado.";

                return RedirectToAction("StatesList");
            }
            catch (LogicException ex)
            {
                TempData["error"] = ex.Message;

                return RedirectToAction("StatesList");
            }
        }


        // Users section


        [HttpGet]
        public async Task<IActionResult> UsersList()
        {
            var users = await _userService.GetUsers();
            var roles = await _userService.GetRoles();
         
            var usersViewModel = _mapper.Map<List<UserViewModel>>(users);
            var rolesViewModel = _mapper.Map<List<RoleViewModel>>(roles);

            foreach(var user in usersViewModel)
            {
                user.LockedOut = user.LockoutEnd > DateTime.UtcNow;
                user.Role = await _userService.GetUserRoleById(user.Id);
            }

            UsersListViewModel viewModel = new()
            {
                UsersList = usersViewModel,
                Roles = rolesViewModel
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UsersListViewModel viewModel)
        {
            try
            {
                var user = _mapper.Map<ApplicationUser>(viewModel);
                user.EmailConfirmed = true;

                var succeeded = await _userService.InsertUser(user, viewModel.Password, viewModel.Role);

                if (succeeded)
                    TempData["success"] = "El usuario se ha creado correctamente.";

                return RedirectToAction("UsersList");
            }
            catch (LogicException ex)
            {
                TempData["error"] = ex.Message;

                return RedirectToAction("UsersList");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserLockState([FromForm] string userId)
        {
            try
            {
                var succeeded = await _userService.ChangeUserLockState(userId);

                if (succeeded)
                    TempData["success"] = "El estado de bloqueo del usuario ha sido cambiado correctamente.";

                return RedirectToAction("UsersList");
            }
            catch (LogicException ex)
            {
                TempData["error"] = ex.Message;

                return RedirectToAction("UsersList");
            }
            catch (BusinessException ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("UsersList");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromForm] string userId)
        {
            try
            {
                var succeeded = await _userService.DeleteUser(userId);

                if (succeeded)
                    TempData["success"] = "El usuario ha sido borrado correctamente.";

                return RedirectToAction("UsersList");
            }
            catch (LogicException ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("UsersList");
            }
            catch (BusinessException ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("UsersList");
            }
        }


        // Suggestions section


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
