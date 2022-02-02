using Microsoft.EntityFrameworkCore;
using SuggestionsApp.Models.Data.Database;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.Interfaces;

namespace SuggestionsApp.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly SuggestionsAppContext _context;

        public CategoriesService(SuggestionsAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();

            return categories;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            return category;
        }

        public Task<bool> InsertCategory(Category suggestion)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateCategory(Category suggestion)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCategory(int id)
        {
            throw new NotImplementedException();
        }
    }
}
