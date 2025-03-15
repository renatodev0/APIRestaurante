using APIRestaurante.Domain.Interfaces;
using APIRestaurante.Domain.Entities;
using APIRestaurante.Application.DTOs;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Order> CreateOrderAsync(CreateOrderDto request)
    {
        var customer = await _unitOfWork.Customers.GetFirstOrDefaultAsync(c => c.PhoneNumber == request.PhoneNumber);
        if (customer == null)
        {
            customer = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber
            };
            await _unitOfWork.Customers.AddAsync(customer);
        }

        var menuItems = await _unitOfWork.MenuItems.GetListAsync(m => request.MenuItemIds.Contains(m.Id));
        if (!menuItems.Any())
            throw new Exception("Nenhum item válido foi encontrado.");

        var order = new Order
        {
            CustomerPhone = customer.PhoneNumber,
            Customer = customer,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            TotalPriceCents = menuItems.Sum(m => (int)m.PriceCents)
        };

        order.OrderItems = menuItems.Select(menuItem => new OrderItem
        {
            MenuItem = menuItem,
            Quantity = 1
        }).ToList();

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.CompleteAsync();

        return order;
    }
    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _unitOfWork.Orders.GetByIdAsync(id);
    }
}
