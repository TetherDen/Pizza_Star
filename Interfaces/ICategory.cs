using Pizza_Star.Models;

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
    }
}
