using Microsoft.AspNetCore.Mvc;
using StockFlow.API.DTOs.Orders;
using StockFlow.API.Models;
using StockFlow.API.Services;

namespace StockFlow.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // POST /api/orders
        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderDto dto)
        {
            var order = await _orderService.PlaceOrderAsync(dto);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        // GET /api/orders?pageNumber=&pageSize=
        [HttpGet]
        public async Task<IActionResult> GetOrders(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var orders = await _orderService.GetAllOrdersAsync(pageNumber, pageSize);
            return Ok(orders);
        }

        // GET /api/orders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound("Order not found");

            return Ok(order);
        }

        // GET /api/orders/status/{status}?pageNumber=&pageSize=
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetOrdersByStatus(
            OrderStatus status,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var orders = await _orderService
                .GetOrdersByStatusAsync(status, pageNumber, pageSize);

            return Ok(orders);
        }

        // GET /api/orders/summary
        [HttpGet("summary")]
        public async Task<IActionResult> GetOrderSummary()
        {
            var summary = await _orderService.GetOrderSummaryAsync();
            return Ok(summary);
        }

        // POST /api/orders/{id}/cancel
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            await _orderService.CancelOrderAsync(id);
            return Ok("Order cancelled successfully");
        }
    }
}
