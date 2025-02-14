using Pizza_Star.Data;
using Pizza_Star.Models.Pages;
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

        //===========
        // l3+
        public async Task<IEnumerable<Product>> GetEightRandomProductsAsync(int productId)
        {
            return await _applicationContext.Products.Where(e => e.Id != productId).OrderBy(e => Guid.NewGuid()).Take(8).ToListAsync();
        }

        public PagedList<Product> GetAllProductsByCategoryWithRatings(QueryOptions options, int categoryId)
        {
            return new PagedList<Product>(_applicationContext.Products.Include(e => e.Category).Include(e => e.Ratings).Where(e => e.CategoryId == categoryId), options);
        }
        //===============================================

        public PagedList<Product> GetAllProducts(QueryOptions options)
        {
            return new PagedList<Product>(_applicationContext.Products.Include(e => e.Category), options);
        }

        public PagedList<Product> GetAllProductsByCategory(QueryOptions options, int categoryId)
        {
            return new PagedList<Product>(_applicationContext.Products.Include(e => e.Category).Where(e => e.CategoryId == categoryId), options);
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

        public async Task<Product> GetProductWithCategoryAndRatingAsync(int id)
        {
            return await _applicationContext.Products.Include(e => e.Category).Include(e => e.Ratings).AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }





    }



}
