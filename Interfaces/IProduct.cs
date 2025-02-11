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

        // ============= new methods  =============

        PagedList<Product> GetAllProductsWithRelations(QueryOptions options);
        Task<Product> GetProductWithCategoryAndRatingAsync(int id);
        PagedList<Product> GetAllProductsByCategoryWithRatings(QueryOptions options, int categoryId);


        // Les 3 add
        PagedList<Product> GetAllProductsByCategory(QueryOptions options, int categoryId);
        Task<IEnumerable<Product>> GetEightRandomProductsAsync(int productId);

        //===================================================

    }
}
