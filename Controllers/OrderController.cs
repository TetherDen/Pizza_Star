using Microsoft.AspNetCore.Mvc;
using Pizza_Star.Interfaces;
using Pizza_Star.Models.Checkout;
using System.Security.Claims;

namespace Pizza_Star.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICart _cart;
        private readonly IOrder _order;

        public OrderController(ICart carts, IOrder orders)
        {
            _cart = carts;
            _order = orders;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cartItems = await _cart.GetShopCartItemsAsync();
            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "ShopCart");
            }

            var order = new Order
            {
                CreatedAt = DateTime.Now
            };

            if (User.Identity.IsAuthenticated)
            {
                order.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", order);
            }

            var cartItems = await _cart.GetShopCartItemsAsync();
            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "ShopCart");
            }

            order.CreatedAt = DateTime.Now;
            if (User.Identity.IsAuthenticated)
            {
                order.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            await _order.AddOrderAsync(order);
            await _cart.ClearCartAsync();

            return RedirectToAction("Success");
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}
