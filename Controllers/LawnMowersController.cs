using GrassShop.Core.Interfaces;
using GrassShop.Core.Services.LawnMowerService.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LawnMowersController(ILawnMowerService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LawnMowerResponseDto>>> GetAll()
    {
        var models = await service.GetAllLawnMowersAsync();
        return Ok(models.Select(ToDto));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LawnMowerResponseDto>> GetById(int id)
    {
        var model = await service.GetLawnMowerByIdAsync(id);
        if (model is null) return NotFound();
        return Ok(ToDto(model));
    }

    [HttpPost]
    public async Task<ActionResult<LawnMowerResponseDto>> Create(CreateLawnMowerArgs args)
    {
        var model = await service.CreateLawnMowerAsync(args);
        return CreatedAtAction(nameof(GetById), new { id = model.Id }, ToDto(model));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateLawnMowerArgs args)
    {
        var model = await service.UpdateLawnMowerAsync(id, args);
        if (model is null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await service.DeleteLawnMowerAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }

    private static LawnMowerResponseDto ToDto(LawnMowerModel model) =>
        new(model.Id, model.Name, model.Brand, model.Description, model.Price, model.Stock);
}
