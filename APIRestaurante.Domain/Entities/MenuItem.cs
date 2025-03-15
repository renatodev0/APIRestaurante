namespace APIRestaurante.Domain.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int PriceCents { get; set; }
    }
}
