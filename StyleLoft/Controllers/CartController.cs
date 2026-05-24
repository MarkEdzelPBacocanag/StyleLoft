using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StyleLoft.Data;
using StyleLoft.Models;
using StyleLoft.ViewModels;

namespace StyleLoft.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == user!.Id)
                .ToListAsync();

            var model = new CartViewModel
            {
                Items = cartItems.Select(ci => new CartItemViewModel
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product!.Name,
                    ImageUrl = ci.Product!.ImageUrl,
                    Price = ci.Product!.Price,
                    Quantity = ci.Quantity,
                    Material = ci.Product!.Material
                }).ToList(),
                StreetAddress = user!.StreetAddress,
                City = user.City,
                StateProvince = user.StateProvince,
                ZipPostalCode = user.ZipPostalCode,
                Country = user.Country
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == user!.Id && ci.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    UserId = user!.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        public async Task<IActionResult> BulkAddToCart(List<int> productIds)
        {
            var user = await _userManager.GetUserAsync(User);
            
            foreach (var productId in productIds)
            {
                var cartItem = await _context.CartItems
                    .FirstOrDefaultAsync(ci => ci.UserId == user!.Id && ci.ProductId == productId);

                if (cartItem == null)
                {
                    cartItem = new CartItem
                    {
                        UserId = user!.Id,
                        ProductId = productId,
                        Quantity = 1
                    };
                    _context.CartItems.Add(cartItem);
                }
                else
                {
                    cartItem.Quantity += 1;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}