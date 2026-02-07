using System.ComponentModel.DataAnnotations;

namespace StockFlow.API.DTOs.Suppliers
{
    public class UpdateSupplierDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}
