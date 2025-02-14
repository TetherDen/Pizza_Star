using Pizza_Star.Models.Pages;
using Pizza_Star.Models;
using System.Collections;

namespace Pizza_Star.Interfaces
{
    public interface ICategory
    {
        IQueryable<Category> GetAll();
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task EditAsync(Category category);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        PagedList<Category> GetAllCategories(QueryOptions options);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        Task<Category?> GetCategoryAsync(int categoryId);
    }
}
