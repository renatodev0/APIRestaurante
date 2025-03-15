using APIRestaurante.Application.Services;
using APIRestaurante.Domain.Entities;
using APIRestaurante.Domain.Interfaces;
using APIRestaurante.Application.DTOs;
using Moq;

public class MenuItemServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMenuItemRepository> _menuItemRepositoryMock;
    private readonly IMenuItemService _menuItemService;

    public MenuItemServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _menuItemRepositoryMock = new Mock<IMenuItemRepository>();

        _unitOfWorkMock.Setup(u => u.MenuItems).Returns(_menuItemRepositoryMock.Object);

        _menuItemService = new MenuItemService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateMenuItem()
    {
        var menuItemDto = new MenuItemCreateDto
        {
            Name = "Pizza",
            PriceCents = 3000
        };

        _menuItemRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<MenuItem>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

        var result = await _menuItemService.CreateAsync(menuItemDto);

        Assert.NotNull(result);
        Assert.Equal("Pizza", result.Name);
        Assert.Equal(3000, result.PriceCents);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMenuItems()
    {
        var menuItems = new List<MenuItem>
    {
        new MenuItem { Id = 1, Name = "Pizza", PriceCents = 3000 },
        new MenuItem { Id = 2, Name = "Burger", PriceCents = 1000 }
    };

        _menuItemRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(menuItems);

        var result = await _menuItemService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, m => m.Name == "Pizza");
        Assert.Contains(result, m => m.Name == "Burger");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMenuItem_WhenExists()
    {
        var menuItem = new MenuItem { Id = 1, Name = "Pizza", PriceCents = 3000 };

        _menuItemRepositoryMock
            .Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(menuItem);

        var result = await _menuItemService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Pizza", result.Name);
        Assert.Equal(3000, result.PriceCents);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowKeyNotFoundException_WhenNotExists()
    {
        _menuItemRepositoryMock
            .Setup(repo => repo.GetByIdAsync(123))
            .ReturnsAsync((MenuItem?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _menuItemService.GetByIdAsync(123));
    }

    [Fact]
    public async Task UpdatePartialAsync_ShouldUpdateMenuItem()
    {
        var existingMenuItem = new MenuItem { Id = 1, Name = "Pizza", PriceCents = 3000 };
        var menuItemUpdateDto = new MenuItemUpdateDto { Name = "Pizza Margherita", PriceCents = 3500 };

        _menuItemRepositoryMock
            .Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(existingMenuItem);

        _unitOfWorkMock.Setup(u => u.MenuItems.UpdatePartialAsync(existingMenuItem))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

        var result = await _menuItemService.UpdatePartialAsync(1, menuItemUpdateDto);

        Assert.NotNull(result);
        Assert.Equal("Pizza Margherita", result.Name);
        Assert.Equal(3500, result.PriceCents);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteMenuItem()
    {
        var menuItem = new MenuItem { Id = 1, Name = "Pizza", PriceCents = 3000 };

        _menuItemRepositoryMock
            .Setup(repo => repo.DeleteAsync(menuItem))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

        await _menuItemService.DeleteAsync(menuItem);

        _menuItemRepositoryMock.Verify(repo => repo.DeleteAsync(menuItem), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }
}
