using APIRestaurante.Domain.Entities;

namespace APIRestaurante.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync(int pageNumber, int pageSize);
        Task<Customer?> GetFirstOrDefaultAsync(Func<Customer, bool> predicate);
        Task AddAsync(Customer customer);
    }
}
