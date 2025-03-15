using APIRestaurante.Application.DTOs;
using APIRestaurante.Domain.Entities;
using APIRestaurante.Domain.Interfaces;
using Moq;

public class OrderServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _orderService = new OrderService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldCreateOrder_WhenValidRequest()
    {
        var createOrderDto = new CreateOrderDto
        {
            PhoneNumber = "123456789",
            FirstName = "João",
            LastName = "Silva",
            Items = new List<OrderItemDto>
            {
                new OrderItemDto { MenuItemId = 1, Quantity = 2 }
            }
        };

        var menuItem = new MenuItem { Id = 1, Name = "Pizza", PriceCents = 3000 };
        var customer = new Customer { PhoneNumber = "123456789", FirstName = "João", LastName = "Silva" };

        _unitOfWorkMock.Setup(u => u.Customers.GetFirstOrDefaultAsync(It.IsAny<Func<Customer, bool>>()))
            .ReturnsAsync(customer);
        _unitOfWorkMock.Setup(u => u.MenuItems.GetListAsync(It.IsAny<Func<MenuItem, bool>>()))
            .ReturnsAsync(new List<MenuItem> { menuItem });

        _unitOfWorkMock.Setup(u => u.Orders.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

        var result = await _orderService.CreateOrderAsync(createOrderDto);

        Assert.NotNull(result);
        Assert.Equal(customer.PhoneNumber, result.CustomerPhone);
        Assert.Single(result.OrderItems);
        Assert.Equal(2, result.OrderItems.First().Quantity);
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldThrowException_WhenMenuItemNotFound()
    {
        var createOrderDto = new CreateOrderDto
        {
            PhoneNumber = "123456789",
            FirstName = "João",
            LastName = "Silva",
            Items = new List<OrderItemDto>
            {
                new OrderItemDto { MenuItemId = 999, Quantity = 1 }
            }
        };

        var customer = new Customer { PhoneNumber = "123456789", FirstName = "João", LastName = "Silva" };

        _unitOfWorkMock.Setup(u => u.Customers.GetFirstOrDefaultAsync(It.IsAny<Func<Customer, bool>>()))
            .ReturnsAsync(customer);
        _unitOfWorkMock.Setup(u => u.MenuItems.GetListAsync(It.IsAny<Func<MenuItem, bool>>()))
            .ReturnsAsync(new List<MenuItem>());

        await Assert.ThrowsAsync<Exception>(() => _orderService.CreateOrderAsync(createOrderDto));
    }

    [Fact]
    public async Task GetOrderByIdAsync_ShouldReturnOrder_WhenExists()
    {
        var customer = new Customer { PhoneNumber = "123456789", FirstName = "João", LastName = "Silva" };
        var order = new Order { Id = 1, CustomerPhone = "123456789", Customer = customer };
        _unitOfWorkMock.Setup(u => u.Orders.GetByIdAsync(1)).ReturnsAsync(order);

        var result = await _orderService.GetOrderByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(order.Id, result.Id);
    }

    [Fact]
    public async Task GetOrderByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        _unitOfWorkMock.Setup(u => u.Orders.GetByIdAsync(999)).ReturnsAsync((Order?)null);

        var result = await _orderService.GetOrderByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateOrderAsync_ShouldUpdateOrder_WhenValidRequest()
    {
        var customer = new Customer { PhoneNumber = "123456789", FirstName = "João", LastName = "Silva" };
        var order = new Order { Id = 1, CustomerPhone = "123456789", Customer = customer, Status = "Pending", TotalPriceCents = 3500 };
        var updateOrderDto = new UpdateOrderDto { Status = "Completed", TotalPriceCents = 3000 };

        _unitOfWorkMock.Setup(u => u.Orders.GetByIdAsync(1)).ReturnsAsync(order);
        _unitOfWorkMock.Setup(u => u.Orders.UpdateAsync(order)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

        var result = await _orderService.UpdateOrderAsync(1, updateOrderDto);

        Assert.NotNull(result);
        Assert.Equal("Completed", result.Status);
        Assert.Equal(3000, result.TotalPriceCents);
    }

    [Fact]
    public async Task UpdateOrderAsync_ShouldThrowException_WhenOrderNotFound()
    {
        _unitOfWorkMock.Setup(u => u.Orders.GetByIdAsync(999)).ReturnsAsync((Order?)null);

        await Assert.ThrowsAsync<Exception>(() => _orderService.UpdateOrderAsync(999, new UpdateOrderDto()));
    }

    [Fact]
    public async Task AddOrderItemAsync_ShouldAddItemToOrder_WhenValidRequest()
    {
        var customer = new Customer { PhoneNumber = "123456789", FirstName = "João", LastName = "Silva" };
        var order = new Order
        {
            Id = 1,
            TotalPriceCents = 1000,
            Customer = customer,
            CustomerPhone = "123456789",
            OrderItems = new List<OrderItem>()
        };
        var addOrderItemDto = new AddOrderItemDto
        {
            Items = new List<OrderItemDto>
        {
            new OrderItemDto { MenuItemId = 1, Quantity = 2 }
        }
        };

        var menuItem = new MenuItem { Id = 1, Name = "Pizza", PriceCents = 2000 };

        _unitOfWorkMock.Setup(u => u.Orders.GetByIdAsync(1)).ReturnsAsync(order);
        _unitOfWorkMock.Setup(u => u.MenuItems.GetByIdAsync(1)).ReturnsAsync(menuItem);
        _unitOfWorkMock.Setup(u => u.OrderItems.AddAsync(It.IsAny<OrderItem>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.Orders.UpdateAsync(order)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

        var result = await _orderService.AddOrderItemAsync(1, addOrderItemDto);

        Assert.NotNull(result);
        Assert.Equal(5000, result.TotalPriceCents);
        Assert.Single(result.OrderItems);
        Assert.Equal(2, result.OrderItems.First().Quantity);
        Assert.Equal(menuItem.Id, result.OrderItems.First().MenuItem.Id);
    }


    [Fact]
    public async Task RemoveOrderItemAsync_ShouldRemoveItemFromOrder_WhenValidRequest()
    {
        // Arrange
        var customer = new Customer { PhoneNumber = "123456789", FirstName = "João", LastName = "Silva" };
        var order = new Order { Id = 1, TotalPriceCents = 3000, Customer = customer, CustomerPhone = "123456789" };
        var orderItem = new OrderItem { Id = 1, Quantity = 1, MenuItem = new MenuItem { Name = "Pizza", PriceCents = 2000 }, OrderId = 1 };

        _unitOfWorkMock.Setup(u => u.Orders.GetByIdAsync(1)).ReturnsAsync(order);
        _unitOfWorkMock.Setup(u => u.OrderItems.GetByIdAsync(1)).ReturnsAsync(orderItem);

        _unitOfWorkMock.Setup(u => u.OrderItems.RemoveAsync(orderItem)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.Orders.UpdateAsync(order)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

        var result = await _orderService.RemoveOrderItemAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1000, result.TotalPriceCents);
    }

    [Fact]
    public async Task RemoveOrderItemAsync_ShouldThrowException_WhenItemNotFound()
    {
        _unitOfWorkMock.Setup(u => u.OrderItems.GetByIdAsync(999)).ReturnsAsync((OrderItem?)null);

        await Assert.ThrowsAsync<Exception>(() => _orderService.RemoveOrderItemAsync(999));
    }
}
