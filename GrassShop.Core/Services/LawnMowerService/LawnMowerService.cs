using GrassShop.Core.Interfaces;
using GrassShop.Core.Services.LawnMowerService.Models;
using GrassShop.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrassShop.Core.Services.LawnMowerService;

public class LawnMowerService(GrassDbContext db) : ILawnMowerService
{
    public async Task<LawnMowerModel> CreateLawnMowerAsync(CreateLawnMowerArgs args)
    {
        var entity = new LawnMower
        {
            Name = args.Name,
            Brand = args.Brand,
            Description = args.Description,
            Price = args.Price,
            Stock = args.Stock
        };

        db.LawnMowers.Add(entity);
        await db.SaveChangesAsync();

        return entity.ToModel();
    }

    public async Task<IEnumerable<LawnMowerModel>> GetAllLawnMowersAsync()
    {
        var entities = await db.LawnMowers.ToListAsync();
        return entities.Select(e => e.ToModel());
    }

    public async Task<LawnMowerModel?> GetLawnMowerByIdAsync(int id)
    {
        var entity = await db.LawnMowers.FindAsync(id);
        return entity is null ? null : entity.ToModel();
    }

    public async Task<LawnMowerModel?> UpdateLawnMowerAsync(int id, UpdateLawnMowerArgs args)
    {
        var entity = await db.LawnMowers.FindAsync(id);
        if (entity is null) return null;

        entity.Name = args.Name;
        entity.Brand = args.Brand;
        entity.Description = args.Description;
        entity.Price = args.Price;
        entity.Stock = args.Stock;

        await db.SaveChangesAsync();

        return entity.ToModel();
    }

    public async Task<bool> DeleteLawnMowerAsync(int id)
    {
        var entity = await db.LawnMowers.FindAsync(id);
        if (entity is null) return false;

        db.LawnMowers.Remove(entity);
        await db.SaveChangesAsync();
        return false;
    }

}
