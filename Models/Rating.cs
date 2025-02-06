namespace Pizza_Star.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string UserId { get; set; } 
        public int RatingValue { get; set; }
        public DateTime RatedAt { get; set; }
    }
}
