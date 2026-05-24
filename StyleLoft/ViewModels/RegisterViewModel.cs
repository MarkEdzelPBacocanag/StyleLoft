using System.ComponentModel.DataAnnotations;

namespace StyleLoft.ViewModels
{
    public class RegisterViewModel
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

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}