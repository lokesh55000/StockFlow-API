using System.ComponentModel.DataAnnotations;

namespace StockFlow.API.DTOs.Orders
{
    public class CreateOrderDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
