using GrassShop.Core.Interfaces;
using GrassShop.Core.Services;
using GrassShop.Core.Services.LawnMowerService;
using GrassShop.Db.Entities;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<GrassDbContext>(opt =>
    opt.UseSqlite("Data Source=lawnshop.db"));
builder.Services.AddScoped<ILawnMowerService, LawnMowerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
