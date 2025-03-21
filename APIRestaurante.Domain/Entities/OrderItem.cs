namespace APIRestaurante.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public required MenuItem MenuItem { get; set; }
        public required int Quantity { get; set; }
    }
}
