using APIRestaurante.Domain.Interfaces;
using APIRestaurante.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace APIRestaurante.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RestaurantContext _context;

        public IMenuItemRepository MenuItems { get; }

        public UnitOfWork(RestaurantContext context, IMenuItemRepository menuItemRepository)
        {
            _context = context;
            MenuItems = menuItemRepository;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
