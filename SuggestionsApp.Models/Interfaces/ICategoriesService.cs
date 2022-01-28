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
        Task<Category> GetCategoryById(int id);
        Task<Category> InsertCategory(Category suggestion);
        Task<Category> UpdateCategory(Category suggestion);
        Task<bool> DeleteCategory(int id);
    }
}
