using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockFlow.API.Data;
using StockFlow.API.DTOs.Products;
using StockFlow.API.Models;

namespace StockFlow.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(AppDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET /api/products?search=&minPrice=&maxPrice=&pageNumber=&pageSize=
        [HttpGet]
        public async Task<IActionResult> GetProducts(
            [FromQuery] string? search,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Products
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search));

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                pageNumber,
                pageSize,
                data = products
            });
        }

        // GET /api/products/low-stock
        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockProducts()
        {
            var products = await _context.Products
                .AsNoTracking()
                .Where(p => p.StockQuantity < 10)
                .ToListAsync();

            return Ok(products);
        }

        // POST /api/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducts), product);
        }

        // PUT /api/products/{id}/stock
        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateProductStock(int id, [FromBody] int stockQuantity)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound("Product not found");

            product.StockQuantity = stockQuantity;
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        // POST /api/products/{id}/restock
        [HttpPost("{id}/restock")]
        public async Task<IActionResult> RestockProduct(int id, [FromBody] RestockProductDto dto)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound("Product not found");

            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == dto.SupplierId);
            if (supplier == null)
                return NotFound("Supplier not found");

            if (!supplier.IsActive)
                return BadRequest("Supplier is not active");

            product.StockQuantity += dto.Quantity;
            product.SupplierId = supplier.Id;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Product restocked. ProductId: {ProductId}, SupplierId: {SupplierId}, QuantityAdded: {Quantity}",
                product.Id, supplier.Id, dto.Quantity);

            return Ok(new
            {
                message = "Product restocked successfully",
                productId = product.Id,
                newStock = product.StockQuantity
            });
        }
    }
}
