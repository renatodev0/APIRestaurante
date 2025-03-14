using APIRestaurante.Domain.Entities;

namespace APIRestaurante.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync(int pageNumber, int pageSize);
    }
}
