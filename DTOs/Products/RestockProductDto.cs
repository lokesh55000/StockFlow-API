using System.ComponentModel.DataAnnotations;

namespace StockFlow.API.DTOs.Products
{
    public class RestockProductDto
    {
        [Required]
        public int SupplierId { get; set; }

        [Required]
        [Range(1, 100000)]
        public int Quantity { get; set; }
    }
}
