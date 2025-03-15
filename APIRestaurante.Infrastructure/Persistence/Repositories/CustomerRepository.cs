using APIRestaurante.Domain.Entities;
using APIRestaurante.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using APIRestaurante.Infrastructure.Data;


namespace APIRestaurante.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly RestaurantContext _context;

        public CustomerRepository(RestaurantContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Customers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public async Task<Customer?> GetFirstOrDefaultAsync(Func<Customer, bool> predicate)
        {
            return await Task.FromResult(_context.Customers.FirstOrDefault(predicate));
        }
    }
}
