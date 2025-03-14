using APIRestaurante.Domain.Entities;

namespace APIRestaurante.Domain.Interfaces
{
    public interface IMenuItemRepository
    {
        Task<MenuItem?> GetByIdAsync(int id);
        Task<IEnumerable<MenuItem>> GetAllAsync();
        Task AddAsync(MenuItem menuItem);
        Task UpdatePartialAsync(MenuItem menuItem);
        Task DeleteAsync(MenuItem menuItem);
    }
}
