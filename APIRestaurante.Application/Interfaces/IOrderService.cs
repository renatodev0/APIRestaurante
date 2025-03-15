using APIRestaurante.Domain.Entities;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(CreateOrderDto request);
    Task<Order?> GetOrderByIdAsync(int id);
}
