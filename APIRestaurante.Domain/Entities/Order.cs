namespace APIRestaurante.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Status { get; set; } = OrderStatus.Pending.ToString();

        public int TotalPriceCents { get; set; }

        public required string CustomerPhone { get; set; }

        public required Customer Customer { get; set; }

        public DateTime UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
