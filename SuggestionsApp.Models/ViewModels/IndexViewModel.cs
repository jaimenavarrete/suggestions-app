using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuggestionsApp.Models.ViewModels
{
    public class IndexViewModel
    {
        public List<SuggestionViewModel> SuggestionsList { get; set; } = null!;

        public List<CategoryViewModel> CategoriesList { get; set; } = null!;
    }
}
