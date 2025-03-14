using APIRestaurante.Application.Interfaces;
using APIRestaurante.Domain.Entities;
using APIRestaurante.Domain.Interfaces;

namespace APIRestaurante.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _unitOfWork.Customers.GetAllAsync(pageNumber, pageSize);
        }
    }
}
