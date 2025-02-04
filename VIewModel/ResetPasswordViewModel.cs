using System.ComponentModel.DataAnnotations;

namespace Lesson_22_Pizza_Star.VIewModel
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        [Required]
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
