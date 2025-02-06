using Lesson_22_Pizza_Star.Data;
using Lesson_22_Pizza_Star.Models.Pages;
using Microsoft.EntityFrameworkCore;
using Pizza_Star.Interfaces;
using Pizza_Star.Models;

namespace Pizza_Star.Repository
{
    public class ProductRepository : IProduct
    {
        private readonly ApplicationContext _applicationContext;


        public ProductRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        // =============  my new methods  =============
        public PagedList<Product> GetAllProductsWithRelations(QueryOptions options)
        {
            var query = _applicationContext.Products
                .Include(e => e.Category)
                .Include(e => e.Ratings)
                .AsQueryable();

            return new PagedList<Product>(query, options);
        }


        //===============================================

        public PagedList<Product> GetAllProducts(QueryOptions options)
        {
            return new PagedList<Product>(_applicationContext.Products.Include(e => e.Category), options);
        }


        public async Task AddProductAsync(Product product)
        {
            await _applicationContext.Products.AddAsync(product);
            await _applicationContext.SaveChangesAsync();
        }


        public async Task DeleteProductAsync(Product product)
        {
            _applicationContext.Products.Remove(product);
            await _applicationContext.SaveChangesAsync();
        }


        public async Task EditProductAsync(Product product)
        {
            _applicationContext.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _applicationContext.Entry(product).Property(e => e.DateOfPublication).IsModified = false;
            await _applicationContext.SaveChangesAsync();
        }


        public async Task<Product> GetProductAsync(int id)
        {
            return await _applicationContext.Products.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<Product> GetProductWithCategoryAsync(int id)
        {
            return await _applicationContext.Products.Include(e => e.Category).AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }
    }



}
