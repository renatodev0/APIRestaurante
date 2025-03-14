namespace APIRestaurante.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMenuItemRepository MenuItems { get; }
        ICustomerRepository Customers { get; }
        Task CompleteAsync();
    }
}
