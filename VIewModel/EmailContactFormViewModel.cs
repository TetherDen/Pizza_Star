using System.ComponentModel.DataAnnotations;

namespace Pizza_Star.VIewModel
{
    public class EmailContactFormViewModel
    {
        [Required(ErrorMessage = "Пожалуйста, введите ваше имя")]
        [StringLength(100, ErrorMessage = "Имя не должно превышать 100 символов")]
        [Display(Name = "Your Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите ваш email")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        [StringLength(100, ErrorMessage = "Email не должен превышать 100 символов")]
        [Display(Name = "E-Mail Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите ваш телефон")]
        [StringLength(15, ErrorMessage = "Номер телефон не должен превышать 15 символов")]
        [Phone(ErrorMessage = "Неверный формат телефона")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите сообщение")]
        [StringLength(255, ErrorMessage = "Сообщение не должно превышать 255 символов")]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}
