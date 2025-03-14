namespace APIRestaurante.Application.DTOs;

public class MenuItemCreateDto
{
    public required string Name { get; set; }
    public required decimal PriceCents { get; set; }
}
