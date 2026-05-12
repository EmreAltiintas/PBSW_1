namespace WebAPI.DTOs;

public record UpdateLawnMowerDto(
    string Name,
    string Brand,
    string Description,
    decimal Price,
    int Stock
);
