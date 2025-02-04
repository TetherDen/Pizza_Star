using System.ComponentModel.DataAnnotations;

namespace Lesson_22_Pizza_Star.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string? Email { get; set; }
        public int Year { get; set; }
        public string? Phone { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? PasswordConfirm { get; set; }
    }
}
