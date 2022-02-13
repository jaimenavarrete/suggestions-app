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
        Task<bool> InsertCategory(Category suggestion);
        Task<bool> UpdateCategory(Category suggestion);
        Task<bool> DeleteCategory(int id);
    }
}
