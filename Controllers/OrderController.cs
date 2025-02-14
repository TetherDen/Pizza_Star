using Pizza_Star.Models;
using Pizza_Star.Models.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pizza_Star.Interfaces;
using Pizza_Star.Models.Checkout;
using Pizza_Star.Repository;
using Pizza_Star.ViewModel;
using System.Security.Claims;
using Pizza_Star.VIewModel;

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
        public async Task<IActionResult> CreateOrder()
        {
            var cartItems = await _cart.GetShopCartItemsAsync();

            if(cartItems.Count() > 0)
            {
                var orderViewModel = new CreateOrderViewModel
                {

                };
                return View("CreateOrder", orderViewModel);
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
                        CreatedAt = DateTime.Now,

                        OrderDetails = cartItems.Select(cartItem => new OrderDetails
                        {
                            ProductId = cartItem.ProductId,
                            Quantity = cartItem.Count
                        }).ToList()
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




        //====================================================================================

        // Admin control panel for Orders:
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ShowOrders(QueryOptions options)
        {

            if (options.SearchPropertyName != null)
            {
                // При выборе статуса "Все статусы" в выпадающем списке, выводим все товары через фейк значение "AllStatusesFakeValue"
                // TODO: Можно Переделать на что-то более нормальное
                if (options.SearchTerm == "AllStatusesFakeValue")
                {
                    options.SearchPropertyName = null;
                    options.SearchTerm = null;
                    var allOrders = _order.GetAllOrdersWithDetails(options);
                    return View(allOrders);

                }
                var orders = _order.GetAllOrdersWithDetails(options);
                return View(orders);
            }
            // Что бы при открытии страницы заказов не выводит все заказы с БД (в тч завершенные и отмененные)
            // выводим только те что pending u proccessing
            var pendingAndProccessOrders = _order.GetPendingAndProcessingOrders(options);
            return View(pendingAndProccessOrders);
        }



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ViewOrder(int id, bool edit = false)
        {
            var order = await _order.GetOrderWithDetailsAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var orderViewModel = new OrderViewModel
            {
                Id = order.Id,
                Fio = order.Fio,
                Phone = order.Phone,
                Email = order.Email,
                City = order.City,
                Address = order.Address,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                OrderDetails = order.OrderDetails ?? new List<OrderDetails>()
            };

            ViewBag.EditMode = edit;    // флаг режима edit   
            return View(orderViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrder(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EditMode = true;
                return View("ViewOrder", model);
            }

            var order = await _order.GetOrderAsync(model.Id);
            if (order == null)
            {
                return NotFound();
            }

            order.Fio = model.Fio;
            order.Phone = model.Phone;
            order.Email = model.Email;
            order.City = model.City;
            order.Address = model.Address;
            order.Status = model.Status;

            await _order.EditOrderAsync(order);

            return RedirectToAction("ViewOrder", new { id = order.Id });
        }



        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _order.GetOrderAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await _order.RemoveOrderAsync(order);
            return Ok();
        }


    }
}
