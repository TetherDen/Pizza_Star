using Pizza_Star.Models;

namespace Pizza_Star.Models.Checkout
{
    public enum OrderStatus
    {
        Pending,    // Ожидает обработки
        Processing, // В обработке
        Completed,  // Завершен
        Canceled    // Отменен
    }

    public class Order
    {
        public int Id { get; set; }
        public string? Fio { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending; // По умолчанию "Ожидает обработки"

        public IEnumerable<OrderDetails> OrderDetails { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }

    }
}
