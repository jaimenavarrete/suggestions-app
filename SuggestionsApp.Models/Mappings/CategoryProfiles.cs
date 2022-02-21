using AutoMapper;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.ViewModels;

namespace SuggestionsApp.Models.Mappings
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
