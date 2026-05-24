namespace StyleLoft.ViewModels
{
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Material { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public decimal Total => Price * Quantity;
    }

    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? StateProvince { get; set; }
        public string? ZipPostalCode { get; set; }
        public string? Country { get; set; }
        public decimal Subtotal => Items.Sum(i => i.Total);
        public decimal Shipping => Subtotal > 5000 ? 0 : 120;
        public decimal Total => Subtotal + Shipping;
    }
}
