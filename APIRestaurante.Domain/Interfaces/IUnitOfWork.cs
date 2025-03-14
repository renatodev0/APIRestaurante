namespace APIRestaurante.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMenuItemRepository MenuItems { get; }
        Task CompleteAsync();
    }
}
