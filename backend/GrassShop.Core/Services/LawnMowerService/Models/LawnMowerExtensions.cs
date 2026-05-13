using GrassShop.Db.Entities;

namespace GrassShop.Core.Services.LawnMowerService.Models;

public static class LawnMowerExtensions
{
    public static LawnMowerModel ToModel(this LawnMower entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Brand = entity.Brand,
        Description = entity.Description,
        Price = entity.Price,
        Stock = entity.Stock
    };
}
