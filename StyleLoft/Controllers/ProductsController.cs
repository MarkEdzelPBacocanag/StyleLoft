using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StyleLoft.Data;
using StyleLoft.Models;

namespace StyleLoft.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string category, string collection)
        {
            var query = _context.Products
                .Where(p => p.IsPublished && p.StockQuantity > 0)
                .AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);

            if (!string.IsNullOrEmpty(collection))
                query = query.Where(p => p.Collection == collection);

            var products = await query.ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.ViewCount++;
            await _context.SaveChangesAsync();

            bool isProductSeller = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null && user.Id == product.SellerId)
                {
                    isProductSeller = true;
                }
            }

            ViewBag.IsProductSeller = isProductSeller;

            return View(product);
        }
    }
}