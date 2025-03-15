using APIRestaurante.Domain.Interfaces;

namespace APIRestaurante.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IOrderRepository Orders { get; }
        IOrderItemRepository OrderItems { get; }
        IMenuItemRepository MenuItems { get; }
        ICustomerRepository Customers { get; }
        Task CompleteAsync();
    }
}
