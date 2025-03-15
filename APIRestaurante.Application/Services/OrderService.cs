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

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _unitOfWork.Orders.GetAllAsync();
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

        var menuItems = await _unitOfWork.MenuItems.GetListAsync(m => request.Items.Select(i => i.MenuItemId).Contains(m.Id));
        if (menuItems.Count() != request.Items.Count)
            throw new Exception("Um ou mais itens do menu não foram encontrados.");

        var order = new Order
        {
            CustomerPhone = customer.PhoneNumber,
            Customer = customer,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        int totalPriceCents = 0;

        order.OrderItems = request.Items.Select(itemDto =>
        {
            var menuItem = menuItems.First(m => m.Id == itemDto.MenuItemId);
            totalPriceCents += menuItem.PriceCents * itemDto.Quantity;

            return new OrderItem
            {
                MenuItem = menuItem,
                Quantity = itemDto.Quantity
            };
        }).ToList();

        order.TotalPriceCents = totalPriceCents;

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.CompleteAsync();

        return order;
    }
    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _unitOfWork.Orders.GetByIdAsync(id);
    }

    public async Task<Order?> UpdateOrderAsync(int id, string userId, UpdateOrderDto request)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        if (order == null) throw new Exception("Pedido não encontrado.");

        if (request.Status != null)
            order.Status = request.Status;

        order.TotalPriceCents = request.TotalPriceCents ?? order.TotalPriceCents;
        order.UpdatedAt = DateTime.UtcNow;
        order.UpdatedBy = userId;

        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.CompleteAsync();

        return order;
    }

    public async Task<Order?> AddOrderItemAsync(int OrderId, string userId, AddOrderItemDto request)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(OrderId);
        if (order == null) throw new Exception("Pedido não encontrado.");

        int totalPriceCents = 0;

        foreach (var item in request.Items)
        {
            var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(item.MenuItemId);
            if (menuItem == null) throw new Exception($"Item do menu com ID {item.MenuItemId} não encontrado.");

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                MenuItem = menuItem,
                Quantity = item.Quantity
            };

            order.OrderItems.Add(orderItem);

            await _unitOfWork.OrderItems.AddAsync(orderItem);

            totalPriceCents += menuItem.PriceCents * item.Quantity;
        }

        order.TotalPriceCents += totalPriceCents;
        order.UpdatedAt = DateTime.UtcNow;
        order.UpdatedBy = userId;

        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.CompleteAsync();

        return order;
    }

    public async Task<Order?> UpdateOrderItemAsync(int ordemItemId, string userId, UpdateOrderItemDto request)
    {
        var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(ordemItemId);
        if (orderItem == null) throw new Exception("Item do pedido não encontrado.");

        var order = await _unitOfWork.Orders.GetByIdAsync(orderItem.OrderId);
        if (order == null) throw new Exception("Pedido não encontrado.");

        order.TotalPriceCents -= orderItem.MenuItem.PriceCents * orderItem.Quantity;
        orderItem.Quantity = request.Quantity;
        order.TotalPriceCents += orderItem.MenuItem.PriceCents * orderItem.Quantity;
        order.UpdatedAt = DateTime.UtcNow;
        order.UpdatedBy = userId;

        await _unitOfWork.OrderItems.UpdateAsync(orderItem);
        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.CompleteAsync();

        return order;
    }

    public async Task<Order?> RemoveOrderItemAsync(int orderItemId, string userId)
    {
        var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(orderItemId);
        if (orderItem == null) throw new Exception("Item do pedido não encontrado.");

        var order = await _unitOfWork.Orders.GetByIdAsync(orderItem.OrderId);
        if (order == null) throw new Exception("Pedido não encontrado.");

        order.TotalPriceCents -= orderItem.MenuItem.PriceCents * orderItem.Quantity;
        order.UpdatedAt = DateTime.UtcNow;
        order.UpdatedBy = userId;

        await _unitOfWork.OrderItems.RemoveAsync(orderItem);
        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.CompleteAsync();

        return order;
    }

}
