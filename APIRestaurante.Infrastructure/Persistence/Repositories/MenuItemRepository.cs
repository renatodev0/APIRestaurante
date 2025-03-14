using APIRestaurante.Domain.Entities;
using APIRestaurante.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using APIRestaurante.Infrastructure.Data;

namespace APIRestaurante.Infrastructure.Persistence.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly RestaurantContext _context;

        public MenuItemRepository(RestaurantContext context)
        {
            _context = context;
        }

        public async Task<MenuItem?> GetByIdAsync(int id)
        {
            return await _context.MenuItems.FindAsync(id);
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _context.MenuItems.ToListAsync();
        }

        public async Task AddAsync(MenuItem menuItem)
        {
            await _context.MenuItems.AddAsync(menuItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePartialAsync(MenuItem menuItem)
        {
            var existingMenuItem = await _context.MenuItems.FirstOrDefaultAsync(x => x.Id == menuItem.Id);

            if (existingMenuItem == null)
                return;

            if (!string.IsNullOrEmpty(menuItem.Name))
                existingMenuItem.Name = menuItem.Name;

            if (menuItem.PriceCents != 0)
                existingMenuItem.PriceCents = menuItem.PriceCents;

            _context.Entry(existingMenuItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(MenuItem menuItem)
        {
            _context.MenuItems.Remove(menuItem);
            return Task.CompletedTask;
        }
    }
}
