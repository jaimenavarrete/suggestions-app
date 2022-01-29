using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SuggestionsApp.Models.Interfaces;
using SuggestionsApp.Models.ViewModels;

namespace SuggestionsApp.WebUI.Controllers
{
    public class SuggestionsController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IMapper _mapper;

        public SuggestionsController(ICategoriesService categoriesService, IMapper mapper)
        {
            _categoriesService = categoriesService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
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
