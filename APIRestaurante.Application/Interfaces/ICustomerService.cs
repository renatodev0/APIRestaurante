using APIRestaurante.Domain.Entities;

namespace APIRestaurante.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync(int pageNumber, int pageSize);
    }
}
