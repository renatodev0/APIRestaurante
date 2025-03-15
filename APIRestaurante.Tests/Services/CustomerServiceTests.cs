using APIRestaurante.Application.Interfaces;
using APIRestaurante.Application.Services;
using APIRestaurante.Domain.Entities;
using APIRestaurante.Domain.Interfaces;
using Moq;

public class CustomerServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly ICustomerService _customerService;

    public CustomerServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();

        _unitOfWorkMock.Setup(u => u.Customers).Returns(_customerRepositoryMock.Object);

        _customerService = new CustomerService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnCustomers()
    {
        var customers = new List<Customer>
        {
            new Customer { Id = 1, FirstName = "João", LastName = "Silva", PhoneNumber = "123456789" },
            new Customer { Id = 2, FirstName = "Maria", LastName = "Souza", PhoneNumber = "987654321" }
        };

        _customerRepositoryMock
            .Setup(repo => repo.GetAllAsync(1, 10))
            .ReturnsAsync(customers);

        var result = await _customerService.GetAllAsync(1, 10);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, c => c.FirstName == "João");
        Assert.Contains(result, c => c.FirstName == "Maria");
    }
}
