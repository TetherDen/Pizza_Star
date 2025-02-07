using Lesson_22_Pizza_Star.Data;
using Lesson_22_Pizza_Star.Models;
using Lesson_22_Pizza_Star.Models.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizza_Star.Interfaces;
using Pizza_Star.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Lesson_22_Pizza_Star.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProduct _products;
        private readonly ApplicationContext _context;
        public HomeController(IProduct product, ApplicationContext context)
        {
            _products = product;
            _context = context;
        }



        //[Route("/")]
        //[HttpGet]
        //public IActionResult Index(QueryOptions options, string? search)
        //{
        //    Console.WriteLine(options.SearchPropertyName);
        //    Console.WriteLine(options.SearchTerm);
        //    //  ароч тут поиск уже есть но надо убрать  прайс и вес    или хз как сделать чтоб по числам было там в пейджет лист смотри завтра...

        //    var products = _products.GetAllProductsWithRelations(options);


        //    return View(products);
        //}

        [Route("/")]
        [HttpGet]
        public IActionResult Index(QueryOptions options, string? search, string? sortBy)
        {
            var products = _products.GetAllProductsWithRelations(options, sortBy);
            return View(products);
        }



        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Rate(int productId, int rating)
        {
            var product = await _context.Products
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return Json(new { success = false, message = "Product not found" });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingRating = product.Ratings
                .FirstOrDefault(r => r.UserId == userId);

            if (existingRating != null)
            {
                existingRating.RatingValue = rating;
                existingRating.RatedAt = DateTime.UtcNow;
            }
            else
            {
                product.Ratings.Add(new Rating
                {
                    ProductId = productId,
                    UserId = userId,
                    RatingValue = rating,
                    RatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();

            // ѕересчитываем значени€ дл€ обнолвени€ отрисовки кол-ва звезд продукта.
            // после новой оценки юзером продуктабез обнолвени€ страницы.
            // “ам JS в представлении надо доделать, пока обновл€тет только при 1ом голосовании, второй голос уже не отрисовует звезды
            var newAverage = product.Ratings.Any()
                ? Math.Round(product.Ratings.Average(r => r.RatingValue), 1)
                : 0;

            return Json(new
            {
                success = true,
                newRating = newAverage,
                ratingCount = product.Ratings.Count
            });
        }
    }


}
