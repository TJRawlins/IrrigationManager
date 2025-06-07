# Droplet Backend - Irrigation & Plant Management API

A robust .NET API backend for the Droplet irrigation and plant management system. Built with ASP.NET Core, it provides comprehensive water usage tracking, zone management, and plant data services. It also provides endpoints for managing devices, schedules, zones, and system status, making it easy to automate and monitor irrigation tasks.

## 🚀 Features

- **Water Usage Analytics**: Calculate and track water consumption per season, zone, and individual plants
- **Zone Management**: CRUD operations for irrigation zones with bulk operations support
- **Plant Management**: Comprehensive plant data management with filtering and export capabilities
- **Water Management**: Scheduling for irrigation events and water calculation services
- **File Upload Support**: Image handling for zones and plants via Firebase integration
- **Bulk Operations**: Efficient data processing using Entity Framework Extensions
- **Secure Configuration**: Secrets management through Infisical

## 🛠️ Technology Stack

- **Framework**: .NET 6/7/8
- **Language**: C#
- **Database**: Microsoft SQL Server
- **ORM**: Entity Framework Core
- **Extensions**: Entity Framework Extensions (for bulk operations)
- **Secrets Management**: Infisical
- **Database Queries**: T-SQL

## 📋 Prerequisites

- .NET SDK 6.0 or later
- Microsoft SQL Server (LocalDB/Express/Full)
- Visual Studio 2022 or VS Code
- Infisical CLI (for secrets management)

## 🔧 Installation & Setup

### 1. Clone the Repository
```bash
git clone https://github.com/TJRawlins/IrrigationManager.git
cd IrrigationManager
```

### 2. Install Dependencies
```bash
dotnet restore
```

### 3. Database Setup
```bash
# Update connection string in appsettings.json
# The context is named IMSContext (Irrigation Management System)
# Run migrations
dotnet ef database update
```

### 4. Configure Secrets (Infisical)
```bash
# Install Infisical CLI
# Login and sync secrets
infisical login
infisical run -- dotnet run
```

### 5. Run the Application
```bash
# Development mode
dotnet run

# Or with hot reload
dotnet watch run
```

### Running in Visual Studio
•	Open droplet.sln in Visual Studio.
•	Press F5 to build and run the project.
•	Use the integrated Test Explorer to run unit tests.

The API will be available at `https://localhost:5555` (or configured port).

## 📁 Project Structure

```
src/
├── Controllers/         # API endpoints
│   ├── BaseApiController.cs
│   ├── AccountController.cs
│   ├── UsersController.cs
│   ├── PlantsController.cs
│   ├── SeasonsController.cs
│   └── ZonesController.cs
├── Models/             # Data models
│   ├── Plant.cs
│   ├── Season.cs
│   ├── Zone.cs
│   └── User.cs
├── DTOs/               # Data transfer objects
│   ├── LoginDto.cs
│   ├── RegisterDto.cs
│   └── UserDto.cs
├── Interfaces/         # Service contracts
│   ├── ICalculationService.cs
│   └── ITokenService.cs
├── Data/               # Database context
│   └── IMSContext.cs
├── Extensions/         # Service registrations
│   ├── ApplicationServiceExtensions.cs
│   ├── IdentityServiceExtensions.cs
│   └── LicenseServiceExtensions.cs
├── Services/           # Business logic implementation
│   ├── CalculationService.cs
│   └── TokenService.cs
├── Migrations/         # EF migrations
└── appsettings.json    # Configuration
```

## 🔌 API Endpoints

### Authentication
- `POST /api/account/register` - User registration
- `POST /api/account/login` - User login
- `POST /api/account/logout` - User logout

### Users
- `GET /api/users/{id}` - Get user profile by ID
- `GET /api/users` - Get all user profiles
- `POST /api/users` - Create new user profile
- `PUT /api/users/{id}` - Update user profile by ID
- `DELETE /api/users/{id}` - Delete user profile by ID

### Seasons
- `GET /api/seasons` - Get all seasons for user
- `GET /api/seasons/{id}` - Get season by ID
- `POST /api/seasons` - Create new season
- `PUT /api/seasons/{id}` - Update season
- `DELETE /api/seasons/{id}` - Delete season

### Zones
- `GET /api/zones` - Get zones by season
- `GET /api/zones/{id}` - Get zone by ID
- `POST /api/zones` - Create new zone
- `PUT /api/zones/{id}` - Update zone
- `DELETE /api/zones/{id}` - Delete zone (cascades to plants)

### Plants
- `GET /api/plants` - Get plants with filtering/pagination
- `GET /api/plants/{id}` - Get plant by ID
- `POST /api/plants` - Create new plant
- `PUT /api/plants/{id}` - Update plant
- `DELETE /api/plants/{id}` - Delete plant
- `POST /api/plants/copyplantstonewzone/{oldZoneId}/{newZoneId}/{seasonId}` - Bulk copy plants operations
- `DELETE /api/plants/deleteplantsfromzone/{zoneId}/{seasonId}` - Bulk delete plants operations

## 💾 Database Schema
Currently using one-to-many relationship. This will be refactored to implement a many-to-many relationship using a junction table (ZonePlant) between plants and zones.

### Key Entities

**Users**
- Id, Firstname, Username, Lastname, Email, ImagePath, PasswordHash, PasswordSalt

**Seasons**
- Id, Name, TimeStamp, TotalGalPerWeek, TotalGalPerMonth, TotalGalPerYear, TotalZones, Zones (virtual)

**Zones**
- Id, Name, Season, ImagePath, RuntimeHours, RuntimeMinutes, RuntimePerWeek, RuntimePerMonth, StartHours, StartMinutes, EndHours, EndMinutes, TimeStamp, TotalGalPerWeek, TotalGalPerMonth, TotalGalPerYear, TotalPlants, SeasonId, Plants (virtual), Seasons (virtual)

**Plants**
- Id, Type, Name, GalsPerWk, GalsPerWkCalc, Quantity, EmittersPerPlant, EmitterGPH, TimeStamp, ImageUrl, Notes, Age, HarvestMonth, Exposure, ZoneId, Zones (virtual)

**Relationships**
- User → Seasons (One-to-Many)
- Season → Zones (One-to-Many)
- Zone → Plants (One-to-Many)

## 🧮 Service Architecture

### Calculation Service
```csharp
// ICalculationService interface
 public interface ICalculationService
 {
     Task RecalculateSeasonGallons(int seasonId, IMSContext context);
     Task RecalculateZoneGallons(int zoneId, IMSContext context);
     Task RecalculateTotalPlants(int zoneId, IMSContext context);
     Task CalculateGallonsPerWeek(int zoneId, IMSContext context);
 }

// Implementation example
public class ZonesController : BaseApiController
{
    private readonly ICalculationService _calculationService;

    [HttpPost]
    public async Task<ActionResult<Models.Zone>> PostZone(Models.Zone zone)
    {
        if (_context.Zones == null)
        {
            return Problem("Entity set 'IMSContext.Zones'  is null.");
        }
        _context.Zones.Add(zone);
        await _context.SaveChangesAsync();
        await _calculationService.RecalculateSeasonGallons(zone.SeasonId, _context);

        return CreatedAtAction("GetZone", new { id = zone.Id }, zone);
}

}
```

### Token Service
```csharp
// ITokenService interface
public interface ITokenService
{
    string CreateToken(User user);
    bool ValidateToken(string token);
}

// JWT implementation
public class TokenService : ITokenService {
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration config)
    {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]!));
    }

    public string CreateToken(User user) {
        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.NameId, user.Username)
        };
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
```

## 🏗️ Architecture & Extensions

### Service Registration
The application uses extension methods to organize service registrations:

**ApplicationServiceExtensions.cs**
```csharp
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
```

**IdentityServiceExtensions.cs**
```csharp
public static class IdentityServiceExtensions {
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, 
        IConfiguration config) {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => {
    #pragma warning disable CS8604 // Possible null reference argument.
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.
                    UTF8.GetBytes(config["TokenKey"])),
                ValidateIssuer = false,
                ValidateAudience = false
            };
    #pragma warning restore CS8604
        });
        return services;
    }
}
```

**LicenseServiceExtensions.cs**
```csharp
public static class LicenseServiceExtension
{
    public static void AddZEntityFrameworkExtenssionsLicense()
    {
        var ClientId = Environment.GetEnvironmentVariable("INFISICAL_DROPLET_CLIENT_ID");
        var ClientSecret = Environment.GetEnvironmentVariable("INFISICAL_DROPLET_CLIENT_SECRET");
        var ProjectId = Environment.GetEnvironmentVariable("INFISICAL_DROPLET_PROJECT_ID");

        ClientSettings settings = new ClientSettings
        {
            Auth = new AuthenticationOptions
            {
                UniversalAuth = new UniversalAuthMethod
                {
                    ClientId = ClientId!,
                    ClientSecret = ClientSecret!
                }
            }
        };

        var infisicalClient = new InfisicalClient(settings);
        var secretLicenseName = infisicalClient.GetSecret(new GetSecretOptions
        {
            SecretName = "Z_ENTITY_FRAMEWORK_EXTENSIONS_LICENSE_NAME",
            ProjectId = ProjectId!,
            Environment = "dev",
        });
        var secretLicenseKey = infisicalClient.GetSecret(new GetSecretOptions
        {
            SecretName = "Z_ENTITY_FRAMEWORK_EXTENSIONS_LICENSE_KEY",
            ProjectId = ProjectId!,
            Environment = "dev",
        });   

        string licenseName = secretLicenseName.SecretValue;
        string licenseKey = secretLicenseKey.SecretValue;

        Z.EntityFramework.Extensions.LicenseManager.AddLicense(licenseName, licenseKey);

        string licenseErrorMessage;
        if (!Z.EntityFramework.Extensions.LicenseManager.ValidateLicense(out licenseErrorMessage))
        {
            throw new Exception(licenseErrorMessage);
        }
    }
}
```

### appsettings.json
```json
{
  "https_port": 443,
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DevDb": "",
    "ProdDb": ""
  }
}
```

## 🧪 Testing

```bash
# Run unit tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 📦 Deployment

### Using Docker
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["IrrigationManager.API.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IrrigationManager.API.dll"]
```

## 🐛 Common Issues & Troubleshooting

**Connection Issues**
- Verify SQL Server is running
- Check connection string format
- Ensure database exists

**Migration Issues**
```bash
# Reset migrations if needed
dotnet ef database drop
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📈 Performance Considerations

- Use Entity Framework Extensions for bulk operations
- Implement pagination for large datasets
- Consider caching for frequently accessed data
- Optimize database queries with proper indexing

## 🔮 Upcoming Features

- Enhanced water usage analytics
- Advanced plant recommendation engine
- Multi-tenant support
- Real-time notifications API
- Integration with external plant databases
- About Page: Central hub for app info, including news, updates, documentation, and support contacts.
- Facebook / Google Login: Simplify account creation and sign-in using social authentication.
- Two-Factor Authentication: Enhance account security with multi-step login options.
- Emitter Calculator: Auto-calculate emitter type and quantity based on plant water needs and zone runtime.
- Custom Dashboard: Visualize seasonal, zone, and plant data through customizable graphs and charts.
- General Settings: User preferences for theme, measurement units (e.g., gallons/liters), and language.
- Account Settings: Manage profile details, password, and advanced security options.
- Multi-User Access: Enable multiple users per account with customizable permissions.
- Plant API Integration: Fetch planting guides, watering tips, and detailed plant data from external sources.
- AI Assistance: Use AI for smart tips, regional planting suggestions, and plant identification via input or images.
- Smart Notifications: Set personalized text or email reminders for watering, fertilizing, pruning, and harvesting.
- Mobile Optimization: Fully responsive design for a seamless experience across all mobile devices.

## 📝 Development Notes

**Started**: September 10, 2023

**Key Design Decisions**:
- Entity Framework for ORM to leverage strong typing and migrations
- Bulk operations support for handling large plant datasets
- Firebase integration for reliable image storage
- Infisical for secure secrets management

---

For frontend implementation, see the [Frontend Repository](link-to-frontend-repo).

## 📞 Support

For issues and questions, please create an issue in this repository or contact the development team.
