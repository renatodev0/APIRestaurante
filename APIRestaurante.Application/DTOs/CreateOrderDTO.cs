public class CreateOrderDto
{
    public required string PhoneNumber { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required List<int> MenuItemIds { get; set; }
}
