using Microsoft.EntityFrameworkCore;
using SuggestionsApp.Models.Data.Database;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.Models.Exceptions;
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

        public async Task<Category?> GetCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            return category;
        }

        public async Task<bool> InsertCategory(Category category)
        {
            if(category is null)
            {
                throw new LogicException("No existe una categoria para crear.");
            }

            _context.Add(category);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            if (category is null)
            {
                throw new LogicException("No existe una categoria para editar.");
            }

            _context.Update(category);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await GetCategoryById(id);

            if (category is null)
            {
                throw new LogicException("No existe una categoría para borrar.");
            }

            _context.Categories.Remove(category);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }
    }
}
