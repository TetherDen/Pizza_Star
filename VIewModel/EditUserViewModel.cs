using System.ComponentModel.DataAnnotations;

namespace Pizza_Star.VIewModel
{
    public class EditUserViewModel
    {
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(256, ErrorMessage = "Email cannot exceed 256 characters.")]
        public string Email { get; set; }

        [Range(1900, 2100, ErrorMessage = "Please enter a valid year.")]
        public int Year { get; set; }


        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        public string PhoneNumber { get; set; }

    }
}
