using Microsoft.EntityFrameworkCore;
using IrrigationManager.Data;
using IrrigationManager.Interfaces;
using IrrigationManager.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<IMSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevDb") ?? throw new InvalidOperationException("Connection string 'DevDb' not found.")));


// Add services to the container.

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddCors();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(x => x.WithOrigins("https://localhost:4200").AllowAnyHeader().AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
