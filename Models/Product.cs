using System.ComponentModel.DataAnnotations;

namespace Pizza_Star.Models
{

    public enum ProductType
    {
        [Display(Name = "Блюдо")]
        Dish,
        [Display(Name = "Напиток")]
        Drink,
        [Display(Name = "Предмет")]
        Subject
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Weight { get; set; }
        public float Calories { get; set; }
        public decimal Price { get; set; }
        public string? Brand { get; set; }
        public string? Image { get; set; }
        public ProductType Type { get; set; }
        public DateTime DateOfPublication { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }


        // нав. свойство для оценок продукта
        public List<Rating> Ratings { get; set; }
        // Сумма всех оценок
        public int RatingSum => Ratings.Sum(r => r.RatingValue);

        // Количество оценок
        public int RatingCount => Ratings.Count;

        // Средний рейтинг
        public double AverageRating => Ratings.Any()
            ? Math.Round(Ratings.Average(r => r.RatingValue), 1)
            : 0;

    }
}
