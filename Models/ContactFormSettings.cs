using System.ComponentModel.DataAnnotations;

namespace Pizza_Star.Models
{
    public class ContactFormSettings
    {
        public int Id { get; set; }

        [EmailAddress(ErrorMessage = "Введите корректный email-адрес.")]
        [StringLength(100, ErrorMessage = "Email не должен превышать 100 символов")]
        public string? ContactEmail { get; set; }
    }
}
