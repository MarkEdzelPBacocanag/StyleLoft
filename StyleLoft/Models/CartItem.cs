namespace StyleLoft.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser? User { get; set; }
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}