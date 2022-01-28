using Microsoft.AspNetCore.Mvc;
using SuggestionsApp.Models.Interfaces;
using SuggestionsApp.Models.ViewModels;
using System.Diagnostics;

namespace SuggestionsApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISuggestionsService _suggestionsService;

        public HomeController(ISuggestionsService suggestionsService)
        {
            _suggestionsService = suggestionsService;
        }

        public async Task<IActionResult> Index()
        {
            var suggestions = await _suggestionsService.GetSuggestions();

            var list = new List<SuggestionViewModel>();

            foreach (var suggestion in suggestions)
            {
                list.Add(new SuggestionViewModel
                {
                    Id = suggestion.Id,
                    Title = suggestion.Title,
                    Description = suggestion.Description,
                    UpvotesAmount = suggestion.UpvotesAmount,
                    CategoryName = suggestion.CategoryId.ToString(),
                    StateName = suggestion.StateId != null ? suggestion.StateId.ToString() : "NULO",
                    UserName = suggestion.UserId.ToString(),
                    Date = suggestion.Date,
                    Approved = suggestion.Approved
                });
            }

            var indexModel = new IndexViewModel
            {
                SuggestionsList = list
            };

            return View(indexModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}