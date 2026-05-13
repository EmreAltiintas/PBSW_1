using GrassShop.Core.Services.LawnMowerService.Models;

namespace GrassShop.Core.Interfaces;

public interface ILawnMowerService
{
    Task<IEnumerable<LawnMowerModel>> GetAllLawnMowersAsync();
    Task<LawnMowerModel?> GetLawnMowerByIdAsync(int id);
    Task<LawnMowerModel> CreateLawnMowerAsync(CreateLawnMowerArgs args);
    Task<LawnMowerModel?> UpdateLawnMowerAsync(int id, UpdateLawnMowerArgs args);
    Task<bool> DeleteLawnMowerAsync(int id);
}
