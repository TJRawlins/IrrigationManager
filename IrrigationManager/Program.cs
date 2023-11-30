using Microsoft.EntityFrameworkCore;
using IrrigationManager.Data;
using IrrigationManager.Interfaces;
using IrrigationManager.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IrrigationManager.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Cors, Token, Connection String
// LOCATION: Services > ApplicationServiceExtensions / IdentityServiceExtensions
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(x => x.WithOrigins("http://localhost:3000", "https://localhost:3000").AllowAnyHeader().AllowAnyMethod());

// STEP #1 Middleware - Have valid token?
app.UseAuthentication();
// STEP #2 Middleware - What is user allowed to do
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
