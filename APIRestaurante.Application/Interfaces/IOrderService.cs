using APIRestaurante.Domain.Entities;
using APIRestaurante.Application.DTOs;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(CreateOrderDto request);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task<Order?> UpdateOrderAsync(int id, string userId, UpdateOrderDto request);
    Task<Order?> AddOrderItemAsync(int id, string userId, AddOrderItemDto request);
    Task<Order?> UpdateOrderItemAsync(int orderItemid, string userId, UpdateOrderItemDto request);
    Task<Order?> RemoveOrderItemAsync(int orderItemid, string userId);
}
