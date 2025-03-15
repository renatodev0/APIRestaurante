using APIRestaurante.Domain.Interfaces;
using APIRestaurante.Domain.Entities;
using APIRestaurante.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace APIRestaurante.Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly RestaurantContext _context;

        public OrderItemRepository(RestaurantContext context)
        {
            _context = context;
        }

        public async Task<OrderItem?> GetByIdAsync(int id)
        {
            return await _context.OrderItems
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task AddAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(OrderItem orderItem)
        {
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
        }
    }
}
