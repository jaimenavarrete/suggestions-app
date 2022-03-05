using Microsoft.AspNetCore.Mvc;

namespace SuggestionsApp.WebUI.Controllers
{
    public class AdminController : Controller
    {
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
    }
}
