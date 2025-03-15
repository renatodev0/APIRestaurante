namespace APIRestaurante.Application.DTOs
{
    public class AddOrderItemDto
    {
        public required List<OrderItemDto> Items { get; set; }
    }
}
