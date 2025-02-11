using Lesson_22_Pizza_Star.Models.Pages;
using Pizza_Star.Models.Checkout;

namespace Pizza_Star.Interfaces
{
    public interface IOrder
    {
        PagedList<Order> GetAllOrdersWithDetails(QueryOptions options);
        PagedList<Order> GetAllOrdersByUserWithDetails(QueryOptions options, string userId);
        Task<Order> GetOrderAsync(int id);

        Task AddOrderAsync(Order order);
        Task EditOrderAsync(Order order);
        Task RemoveOrderAsync(Order order);
    }
}
