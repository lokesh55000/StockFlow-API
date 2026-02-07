using StockFlow.API.DTOs.Orders;
using StockFlow.API.Models;

namespace StockFlow.API.Services
{
    public interface IOrderService
    {
        Task<Order> PlaceOrderAsync(CreateOrderDto dto);
        Task<List<Order>> GetAllOrdersAsync(int pageNumber, int pageSize);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status, int pageNumber, int pageSize);
        Task<OrderSummaryDto> GetOrderSummaryAsync();
        Task CancelOrderAsync(int orderId);
    }
}
