using System.ComponentModel.DataAnnotations;

namespace StockFlow.API.DTOs.Products
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, 1000000)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 100000)]
        public int StockQuantity { get; set; }
    }
}
