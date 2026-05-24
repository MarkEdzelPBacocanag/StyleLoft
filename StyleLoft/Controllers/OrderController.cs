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
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> History()
        {
            var user = await _userManager.GetUserAsync(User);
            var orders = await _context.Orders
                .Include(o => o.OrderItems!)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == user!.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var model = orders.Select(o => new OrderViewModel
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                OrderDate = o.OrderDate,
                Total = o.Subtotal + o.ShippingCost,
                Status = o.Status,
                ItemCount = o.OrderItems!.Count
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartViewModel cart)
        {
            var user = await _userManager.GetUserAsync(User);

            var userCartItems = await _context.CartItems
                .Where(ci => ci.UserId == user!.Id)
                .ToListAsync();

            if (!userCartItems.Any())
            {
                return RedirectToAction(nameof(Index), "Cart");
            }

            var order = new Order
            {
                OrderNumber = $"SL-{DateTime.Now:yyyyMMdd}-{Random.Shared.Next(10000, 99999)}",
                UserId = user!.Id,
                StreetAddress = cart.StreetAddress,
                City = cart.City,
                StateProvince = cart.StateProvince,
                ZipPostalCode = cart.ZipPostalCode,
                Country = cart.Country,
                Subtotal = cart.Subtotal,
                ShippingCost = cart.Shipping,
                Tax = 0,
                Total = cart.Total,
                Status = "Pending"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == user!.Id)
                .ToListAsync();

            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product!.Price
                };
                _context.OrderItems.Add(orderItem);

                item.Product!.StockQuantity -= item.Quantity;
                _context.CartItems.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Confirmation), new { id = order.Id });
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems!)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            return View(order);
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = await _context.Orders
                .Include(o => o.OrderItems!)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == user!.Id);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}