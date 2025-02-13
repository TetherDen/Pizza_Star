using System.ComponentModel.DataAnnotations;

namespace Pizza_Star.VIewModel
{
    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "ФИО обязательно")]
        [StringLength(100, ErrorMessage = "ФИО не должно превышать 100 символов")]
        public string Fio { get; set; }

        [Required(ErrorMessage = "Город обязателен")]
        [StringLength(100, ErrorMessage = "Город не должен превышать 100 символов")]
        public string City { get; set; }

        [Required(ErrorMessage = "Адрес обязателен")]
        [StringLength(250, ErrorMessage = "Адрес не должен превышать 250 символов")]
        public string Address { get; set; }
    }
}
