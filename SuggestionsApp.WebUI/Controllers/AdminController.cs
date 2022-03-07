﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AdminController(
            ISuggestionsService suggestionsService,
            ICategoriesService categoriesService,
            IMapper mapper,
            IUserService userService
            )
        {
            _suggestionsService = suggestionsService;
            _categoriesService = categoriesService;
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

        public IActionResult StatesList()
        {
            return View();
        }

        // Users section

        public IActionResult UsersList()
        {
            return View();
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
