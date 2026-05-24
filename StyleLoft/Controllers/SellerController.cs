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
    public class SellerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public SellerController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        public async Task<IActionResult> Atelier()
        {
            var user = await _userManager.GetUserAsync(User);
            var products = await _context.Products
                .Where(p => p.SellerId == user!.Id)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            var model = new SellerAtelierViewModel
            {
                PublishedProducts = products.Where(p => p.IsPublished).ToList(),
                DraftProducts = products.Where(p => p.IsDraft).ToList(),
                Orders = new List<Order>()
            };

            return View(model);
        }

        public class SellerOrderViewModel
        {
            public Order Order { get; set; } = null!;
            public List<OrderItem> SellerOrderItems { get; set; } = new();
            public decimal SellerTotal { get; set; }
        }
        
        public async Task<IActionResult> Orders()
        {
            var user = await _userManager.GetUserAsync(User);
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .Where(o => o.OrderItems.Any(oi => oi.Product.SellerId == user!.Id))
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var viewModels = orders.Select(order => 
            {
                var sellerItems = order.OrderItems!
                    .Where(oi => oi.Product.SellerId == user!.Id)
                    .ToList();
                
                return new SellerOrderViewModel
                {
                    Order = order,
                    SellerOrderItems = sellerItems,
                    SellerTotal = sellerItems.Sum(oi => oi.Price * oi.Quantity)
                };
            }).ToList();

            return View(viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Add validation errors to TempData and redirect
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    TempData["Error"] = error.ErrorMessage;
                    break; // Just show the first error
                }
                return RedirectToAction(nameof(Atelier));
            }

            var user = await _userManager.GetUserAsync(User);

            string? imageUrl = null;
            if (model.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");

                // Ensure directory exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{model.ImageFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                imageUrl = $"/images/products/{uniqueFileName}";
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Material = model.Material,
                Collection = model.Collection,
                Category = model.Category,
                StockQuantity = model.StockQuantity,
                ImageUrl = imageUrl!,
                SellerId = user!.Id,
                IsPublished = true,
                IsDraft = false
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Product created successfully!";
            return RedirectToAction(nameof(Atelier));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.SellerId == user!.Id);

            if (product == null)
                return NotFound();

            var model = new CreateProductViewModel
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Material = product.Material,
                Collection = product.Collection,
                Category = product.Category,
                StockQuantity = product.StockQuantity
            };

            ViewBag.ProductId = id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateProductViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.SellerId == user!.Id);

            if (product == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.ProductId = id;
                return View(model);
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Material = model.Material;
            product.Collection = model.Collection;
            product.Category = model.Category;
            product.StockQuantity = model.StockQuantity;

            if (model.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{model.ImageFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                product.ImageUrl = $"/images/products/{uniqueFileName}";
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Product updated successfully!";
            return RedirectToAction(nameof(Atelier));
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id && o.OrderItems.Any(oi => oi.Product.SellerId == user!.Id));

            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int id, string status)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id && o.OrderItems.Any(oi => oi.Product.SellerId == user!.Id));

            if (order == null)
                return NotFound();

            order.Status = status;
            if (status == "Delivered")
            {
                order.DeliveredDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Order status updated successfully!";
            return RedirectToAction(nameof(OrderDetails), new { id = order.Id });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatusAjax(int id, string status)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id && o.OrderItems.Any(oi => oi.Product.SellerId == user!.Id));

            if (order == null)
                return Json(new { success = false, message = "Order not found" });

            order.Status = status;
            if (status == "Delivered")
            {
                order.DeliveredDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, newStatus = status, deliveredDate = order.DeliveredDate?.ToString("MMM dd, yyyy HH:mm") });
        }

        public class SellerAtelierViewModel
        {
            public List<Product> PublishedProducts { get; set; } = new();
            public List<Product> DraftProducts { get; set; } = new();
            public List<Order> Orders { get; set; } = new();
        }
    }
}