using SuggestionsApp.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuggestionsApp.Models.Interfaces
{
    public interface ICategoriesService
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category?> GetCategoryById(int id);
        Task<bool> InsertCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        Task<bool> DeleteCategory(int id);
    }
}
