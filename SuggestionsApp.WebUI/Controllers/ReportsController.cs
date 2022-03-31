using System.Collections;
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
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ReportsController(
            ICategoriesService categoriesService,
            IStatesService statesService,
            IUserService userService,
            IMapper mapper)
        {
            _categoriesService = categoriesService;
            _statesService = statesService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> PrintCategories()
        {
            var categories = await _categoriesService.GetCategories();
            var categoriesViewModel = _mapper.Map<List<CategoryViewModel>>(categories);
            return CreatePdf(categoriesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PrintStates()
        {
            var states = await _statesService.GetStates();
            var statesViewModel = _mapper.Map<List<StateViewModel>>(states);
            return CreatePdf(statesViewModel);
        }
        
        [HttpGet]
        public async Task<IActionResult> PrintUsers()
        {
            var users = await _userService.GetUsers();
            var usersViewModel = _mapper.Map<List<UserViewModel>>(users);

            foreach (var user in usersViewModel)
            {
                user.LockedOut = user.LockoutEnd > DateTime.UtcNow;
                user.Role = await _userService.GetUserRoleById(user.Id);
            }
            
            return CreatePdf(usersViewModel);
        }

        #region HelperMethods

        private ViewAsPdf CreatePdf(object list)
        {
            return new ViewAsPdf(list)
            {
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 10"
            };
        }

        #endregion
    }
}
