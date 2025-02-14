using Pizza_Star.Models.Checkout;
using System.ComponentModel.DataAnnotations;

namespace Pizza_Star.VIewModel
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите ФИО")]
        public string? Fio { get; set; }

        [Required(ErrorMessage = "Введите номер телефона")]
        [Phone(ErrorMessage = "Некорректный номер телефона")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Введите Email")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Введите город")]
        public string City { get; set; }

        [Required(ErrorMessage = "Введите адрес")]
        public string Address { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public IEnumerable<OrderDetails>? OrderDetails { get; set; }
    }

}
