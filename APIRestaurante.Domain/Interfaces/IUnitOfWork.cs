namespace APIRestaurante.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IOrderRepository Orders { get; }
        IMenuItemRepository MenuItems { get; }
        ICustomerRepository Customers { get; }
        Task CompleteAsync();
    }
}
