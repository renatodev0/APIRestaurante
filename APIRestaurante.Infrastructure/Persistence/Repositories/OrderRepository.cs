using APIRestaurante.Domain.Entities;
using APIRestaurante.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using APIRestaurante.Infrastructure.Data;


namespace APIRestaurante.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly RestaurantContext _context;

        public OrderRepository(RestaurantContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }
    }
}
