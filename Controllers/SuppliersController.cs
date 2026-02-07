using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockFlow.API.Data;
using StockFlow.API.DTOs.Suppliers;
using StockFlow.API.Models;

namespace StockFlow.API.Controllers
{
    [ApiController]
    [Route("api/suppliers")]
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuppliersController(AppDbContext context)
        {
            _context = context;
        }

        // POST /api/suppliers
        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierDto dto)
        {
            var supplier = new Supplier
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSupplierById), new { id = supplier.Id }, supplier);
        }

        // GET /api/suppliers
        [HttpGet]
        public async Task<IActionResult> GetSuppliers([FromQuery] bool? activeOnly)
        {
            var query = _context.Suppliers
                .AsNoTracking()
                .AsQueryable();

            if (activeOnly == true)
                query = query.Where(s => s.IsActive);

            var suppliers = await query.ToListAsync();
            return Ok(suppliers);
        }


        // GET /api/suppliers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null)
                return NotFound("Supplier not found");

            return Ok(supplier);
        }

        // PUT /api/suppliers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierDto dto)
        {
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null)
                return NotFound("Supplier not found");

            supplier.Name = dto.Name;
            supplier.Email = dto.Email;
            supplier.Phone = dto.Phone;

            await _context.SaveChangesAsync();

            return Ok(supplier);
        }

        // POST /api/suppliers/{id}/deactivate
        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> DeactivateSupplier(int id)
        {
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null)
                return NotFound("Supplier not found");

            supplier.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok("Supplier deactivated");
        }

        // GET /api/suppliers/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveSuppliers()
        {
            var suppliers = await _context.Suppliers
                .Where(s => s.IsActive)
                .ToListAsync();

            return Ok(suppliers);
        }
    }
}

