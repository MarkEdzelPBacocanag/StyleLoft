namespace StyleLoft.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Material { get; set; } = string.Empty;
        public string Collection { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDraft { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string SellerId { get; set; } = string.Empty;
        public virtual ApplicationUser? Seller { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}
