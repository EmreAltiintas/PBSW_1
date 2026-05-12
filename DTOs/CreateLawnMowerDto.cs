namespace WebAPI.DTOs;

public record CreateLawnMowerDto(
    string Name,
    string Brand,
    string Description,
    decimal Price,
    int Stock
);
