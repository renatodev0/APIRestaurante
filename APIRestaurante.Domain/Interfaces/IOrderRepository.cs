using APIRestaurante.Domain.Entities;

namespace APIRestaurante.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
    }
}
