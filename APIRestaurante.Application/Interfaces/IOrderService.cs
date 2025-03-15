using APIRestaurante.Domain.Entities;
using APIRestaurante.Application.DTOs;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(CreateOrderDto request);
    Task<Order?> GetOrderByIdAsync(int id);
    Task<Order?> UpdateOrderAsync(int id, UpdateOrderDto request);
    Task<Order?> AddOrderItemAsync(int id, AddOrderItemDto request);
    Task<Order?> UpdateOrderItemAsync(int orderItemid, UpdateOrderItemDto request);
    Task<Order?> RemoveOrderItemAsync(int orderItemid);
}
