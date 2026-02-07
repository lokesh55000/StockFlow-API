using Microsoft.EntityFrameworkCore;
using StockFlow.API.Data;
using StockFlow.API.DTOs.Orders;
using StockFlow.API.Models;

namespace StockFlow.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderService> _logger;

        public OrderService(AppDbContext context, ILogger<OrderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Order> PlaceOrderAsync(CreateOrderDto dto)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == dto.ProductId);

            if (product == null)
                throw new Exception("Product not found");

            if (product.StockQuantity < dto.Quantity)
                throw new Exception("Insufficient stock");

            var order = new Order
            {
                ProductId = product.Id,
                Quantity = dto.Quantity,
                TotalAmount = product.Price * dto.Quantity,
                Status = OrderStatus.Placed,
                CreatedAt = DateTime.UtcNow
            };

            product.StockQuantity -= dto.Quantity;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Order placed. OrderId: {OrderId}, ProductId: {ProductId}, Quantity: {Quantity}, TotalAmount: {TotalAmount}",
                order.Id, order.ProductId, order.Quantity, order.TotalAmount);

            return order;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _context.Orders
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<OrderSummaryDto> GetOrderSummaryAsync()
        {
            var totalOrders = await _context.Orders.CountAsync();

            var totalRevenue = await _context.Orders
                .Where(o => o.Status == OrderStatus.Placed)
                .SumAsync(o => o.TotalAmount);

            return new OrderSummaryDto
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue
            };
        }

        public async Task CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Order not found");

            if (order.Status == OrderStatus.Cancelled)
                throw new Exception("Order already cancelled");

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == order.ProductId);

            if (product == null)
                throw new Exception("Product not found");

            product.StockQuantity += order.Quantity;
            order.Status = OrderStatus.Cancelled;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Order cancelled. OrderId: {OrderId}, ProductId: {ProductId}, QuantityRestored: {Quantity}",
                order.Id, order.ProductId, order.Quantity);
        }
    }
}
