using Lesson_22_Pizza_Star.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pizza_Star.Interfaces;
using Pizza_Star.Models.Checkout;
using Pizza_Star.Repository;
using Pizza_Star.VIewModel;
using System.Security.Claims;

namespace Pizza_Star.Controllers
{
    public class OrderController : Controller
    {
        private readonly CartRepository _cart;
        private readonly IOrder _order;
        private readonly UserManager<User> _userManager;

        public OrderController(CartRepository carts, IOrder orders, UserManager<User> userManager)
        {
            _cart = carts;
            _order = orders;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cartItems = await _cart.GetShopCartItemsAsync();

            if(cartItems.Count() > 0)
            {
                var orderViewModel = new CreateOrderViewModel
                {

                };
                return View(orderViewModel);
            }
            return RedirectToAction("Index", "ShopCart");
        }

        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateOrder(CreateOrderViewModel model)
        {
            if(ModelState.IsValid)
            {
                var cartItems = await _cart.GetShopCartItemsAsync();
                if (cartItems.Count() > 0)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var order = new Order
                    {
                        Phone = user.PhoneNumber,
                        Email = user.Email,
                        UserId = user.Id,

                        Fio = model.Fio,
                        City = model.City,
                        Address = model.Address,
                        CreatedAt = DateTime.Now
                    };

                    await _order.AddOrderAsync(order);
                    await _cart.ClearCartAsync();

                    return RedirectToAction("Success");
                }
                return RedirectToAction("Index", "ShopCart");
            }
            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }



    }
}
