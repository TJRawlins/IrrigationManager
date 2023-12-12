using IrrigationManager.Data;
using IrrigationManager.Interfaces;
using IrrigationManager.Services;
using Microsoft.EntityFrameworkCore;

namespace IrrigationManager.Extensions {
    // make static so it doesn't get instantiated
    public static class ApplicationServiceExtensions {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, 
            IConfiguration config) 
        {
            services.AddDbContext<IMSContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DevDb") ?? throw new InvalidOperationException("Connection string 'DevDb' not found.")));
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
