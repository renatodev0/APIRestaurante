using APIRestaurante.Domain.Entities;

namespace APIRestaurante.Domain.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<OrderItem?> GetByIdAsync(int id);
        Task AddAsync(OrderItem orderItem);
        Task UpdateAsync(OrderItem orderItem);
        Task RemoveAsync(OrderItem orderItem);
    }
}
