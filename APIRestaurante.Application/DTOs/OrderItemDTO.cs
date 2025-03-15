namespace APIRestaurante.Application.DTOs;

public class OrderItemDto
{
    public required int MenuItemId { get; set; }
    public required int Quantity { get; set; }
}