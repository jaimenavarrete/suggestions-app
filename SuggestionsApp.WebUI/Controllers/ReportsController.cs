using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using SuggestionsApp.Models.Interfaces;
using SuggestionsApp.WebUI.ViewModels;

namespace SuggestionsApp.WebUI.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IStatesService _statesService;
        private readonly IMapper _mapper;

        public ReportsController(
            ICategoriesService categoriesService,
            IStatesService statesService,
            IMapper mapper)
        {
            _categoriesService = categoriesService;
            _statesService = statesService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> PrintCategories()
        {
            var categories = await _categoriesService.GetCategories();
            var categoriesViewModel = _mapper.Map<List<CategoryViewModel>>(categories);

            return new ViewAsPdf(categoriesViewModel)
            {
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 10"
            };
        }

        [HttpGet]
        public async Task<IActionResult> PrintStates()
        {
            var states = await _statesService.GetStates();
            var statesViewModel = _mapper.Map<List<StateViewModel>>(states);

            return new ViewAsPdf(statesViewModel)
            {
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 10"
            };
        }
    }
}
