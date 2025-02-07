using Lesson_22_Pizza_Star.Models.Pages;
using Pizza_Star.Models;

namespace Pizza_Star.Interfaces
{
    public interface IProduct
    {
        PagedList<Product> GetAllProducts(QueryOptions options);
        Task<Product> GetProductAsync (int id);
        Task<Product> GetProductWithCategoryAsync(int id);
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task EditProductAsync(Product product);

        // =============  my new methods  =============

        //PagedList<Product> GetAllProductsWithRelations(QueryOptions options);
        PagedList<Product> GetAllProductsWithRelations(QueryOptions options, string? sortBy);

        //===================================================

    }
}
