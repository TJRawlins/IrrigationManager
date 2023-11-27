using Microsoft.EntityFrameworkCore;
using IrrigationManager.Data;
using IrrigationManager.Interfaces;
using IrrigationManager.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<IMSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevDb") ?? throw new InvalidOperationException("Connection string 'DevDb' not found.")));


// Add services to the container.

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddCors();

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
#pragma warning disable CS8604 // Possible null reference argument.
        options.TokenValidationParameters = new TokenValidationParameters {
            // Make sure token is signed by issueer
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.
                UTF8.GetBytes(builder.Configuration["TokenKey"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
#pragma warning restore CS8604 // Possible null reference argument.
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(x => x.WithOrigins("https://localhost:4200").AllowAnyHeader().AllowAnyMethod());

// #1 Middleware - Have valid token?
app.UseAuthentication();
// #2 Middleware - What is user allowed to do
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
