using APIRestaurante.Domain.Interfaces;
using APIRestaurante.Domain.Entities;
using APIRestaurante.Application.DTOs;

namespace APIRestaurante.Application.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenuItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MenuItem> CreateAsync(MenuItemCreateDto menuItemDto)
        {
            var menuItem = new MenuItem
            {
                Name = menuItemDto.Name,
                PriceCents = menuItemDto.PriceCents
            };

            await _unitOfWork.MenuItems.AddAsync(menuItem);
            await _unitOfWork.CompleteAsync();

            return menuItem;
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _unitOfWork.MenuItems.GetAllAsync();
        }

        public async Task<MenuItem> GetByIdAsync(int id)
        {
            var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(id);
            if (menuItem == null)
            {
                throw new KeyNotFoundException("Menu item not found");
            }

            return menuItem;
        }

        public async Task<MenuItem?> UpdatePartialAsync(int id, MenuItemUpdateDto menuItem)
        {
            var existingMenuItem = await _unitOfWork.MenuItems.GetByIdAsync(id);
            if (existingMenuItem == null)
                return null;

            if (!string.IsNullOrEmpty(menuItem.Name))
                existingMenuItem.Name = menuItem.Name;

            if (menuItem.PriceCents.HasValue)
                existingMenuItem.PriceCents = menuItem.PriceCents.Value;

            await _unitOfWork.MenuItems.UpdatePartialAsync(existingMenuItem);

            await _unitOfWork.CompleteAsync();

            return existingMenuItem;
        }

        public async Task DeleteAsync(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem));

            await _unitOfWork.MenuItems.DeleteAsync(menuItem);
            await _unitOfWork.CompleteAsync();
        }
    }

}
