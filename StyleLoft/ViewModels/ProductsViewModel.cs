using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StyleLoft.ViewModels
{
    public class ProductsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Material { get; set; } = string.Empty;
        public string Collection { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
    }

    public class CreateProductViewModel
    {
        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Display(Name = "Material / Fabric")]
        public string Material { get; set; } = string.Empty;

        public string Collection { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        public IFormFile? ImageFile { get; set; }
    }

    public class EditProfileViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Street Address")]
        public string? StreetAddress { get; set; }

        [Display(Name = "City")]
        public string? City { get; set; }

        [Display(Name = "State/Province")]
        public string? StateProvince { get; set; }

        [Display(Name = "ZIP/Postal Code")]
        public string? ZipPostalCode { get; set; }

        [Display(Name = "Country")]
        public string? Country { get; set; }

        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmNewPassword { get; set; }

        public IFormFile? ProfileImageFile { get; set; }
        
        public bool IsSeller { get; set; }
    }
}