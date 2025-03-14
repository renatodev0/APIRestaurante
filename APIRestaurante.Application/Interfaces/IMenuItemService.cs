using APIRestaurante.Application.DTOs;
using APIRestaurante.Domain.Entities;

namespace APIRestaurante.Domain.Interfaces
{
    public interface IMenuItemService
    {
        Task<MenuItem> GetByIdAsync(int id);
        Task<IEnumerable<MenuItem>> GetAllAsync();
        Task<MenuItem> CreateAsync(MenuItemCreateDto menuItemDto);
        Task<MenuItem?> UpdatePartialAsync(int id, MenuItemUpdateDto menuItem);
        Task DeleteAsync(MenuItem menuItem);
    }
}
