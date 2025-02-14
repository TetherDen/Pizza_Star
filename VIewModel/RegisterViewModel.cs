using System.ComponentModel.DataAnnotations;

namespace Lesson_22_Pizza_Star.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        public string? Email { get; set; }
        [Range(1900, 9999, ErrorMessage = "Year must be between 1900 and 9999.")]
        public int Year { get; set; }
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [StringLength(100, ErrorMessage = "Phone number must not exceed 100 characters.")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password must be between 6 and 100 characters long.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [StringLength(100, ErrorMessage = "Password confirmation must be between 6 and 100 characters long.")]
        [DataType(DataType.Password)]
        public string? PasswordConfirm { get; set; }
    }
}
