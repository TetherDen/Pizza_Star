﻿using Lesson_22_Pizza_Star.Models;

namespace Pizza_Star.Models.Checkout
{
    public class Order
    {
        public int Id { get; set; }
        public string? Fio { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }

        public IEnumerable<OrderDetails> OrderDetails { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
