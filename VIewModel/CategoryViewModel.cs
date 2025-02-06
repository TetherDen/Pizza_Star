using System.ComponentModel.DataAnnotations;

namespace Pizza_Star.VIewModel
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название категории обязательно")]
        [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
        public string? Description { get; set; }

        [Display(Name = "Изображение")]
        public IFormFile? ImageFile { get; set; }

        // для отображения существующего изображения при редактировании
        public string? ExistingImage { get; set; }
    }
}
