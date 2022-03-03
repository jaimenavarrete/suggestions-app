using AutoMapper;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.WebUI.ViewModels;

namespace SuggestionsApp.WebUI.Mappings
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();
        }
    }
}
