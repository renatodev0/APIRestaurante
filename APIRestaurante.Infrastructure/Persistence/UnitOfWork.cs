using APIRestaurante.Domain.Interfaces;
using APIRestaurante.Infrastructure.Data;

namespace APIRestaurante.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RestaurantContext _context;

        public IMenuItemRepository MenuItems { get; }
        public ICustomerRepository Customers { get; }

        public IOrderRepository Orders { get; }

        public IOrderItemRepository OrderItems { get; }

        public UnitOfWork(RestaurantContext context, IMenuItemRepository menuItemRepository, ICustomerRepository customerRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _context = context;
            Orders = orderRepository;
            OrderItems = orderItemRepository;
            MenuItems = menuItemRepository;
            Customers = customerRepository;
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
