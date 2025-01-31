using IrrigationManager.Data;

namespace IrrigationManager.Interfaces
{
    public interface ICalculationService
    {
        Task RecalculateSeasonGallons(int seasonId, IMSContext context);
        Task RecalculateZoneGallons(int zoneId, IMSContext context);
        Task RecalculateTotalPlants(int zoneId, IMSContext context);
        Task CalculateGallonsPerWeek(int zoneId, IMSContext context);
    }
}
