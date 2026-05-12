using Microsoft.EntityFrameworkCore;

namespace GrassShop.Db.Entities;

public class GrassDbContext : DbContext
{
    public GrassDbContext(DbContextOptions<GrassDbContext> options) : base(options) { }

    public DbSet<LawnMower> LawnMowers => Set<LawnMower>();
}
