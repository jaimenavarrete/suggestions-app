namespace SuggestionsApp.WebUI.ViewModels
{
    public class CategoriesListViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public List<CategoryViewModel> CategoriesList { get; set; } = null!;
    }
}
