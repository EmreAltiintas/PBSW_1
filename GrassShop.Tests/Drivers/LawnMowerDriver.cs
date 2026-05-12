using GrassShop.Core.Services.LawnMowerService;
using GrassShop.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrassShop.Tests.Drivers;

public class LawnMowerDriver : IDisposable
{
    public GrassDbContext DbContext { get; }
    public LawnMowerService Service { get; }

    public LawnMowerDriver()
    {
        var options = new DbContextOptionsBuilder<GrassDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        DbContext = new GrassDbContext(options);
        Service = new LawnMowerService(DbContext);
    }

    public void Dispose() => DbContext.Dispose();
}
