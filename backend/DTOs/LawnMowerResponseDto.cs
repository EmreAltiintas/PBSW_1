namespace WebAPI.DTOs;

public record LawnMowerResponseDto(
    int Id,
    string Name,
    string Brand,
    string Description,
    decimal Price,
    int Stock
);
