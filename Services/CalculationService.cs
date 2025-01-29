using IrrigationManager.Data;
using IrrigationManager.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IrrigationManager.Services
{
    public class CalculationService : ICalculationService
    {
        public async Task RecalculateSeasonGallons(int seasonId, IMSContext context)
        {
            var totalWeek = context.Zones
                .Where(zone => zone.SeasonId == seasonId)
                .Sum(zone => zone.TotalGalPerWeek);
            var totalMonth = context.Zones
                .Where(zone => zone.SeasonId == seasonId)
                .Sum(zone => zone.TotalGalPerMonth);
            var totalYear = context.Zones
                .Where(zone => zone.SeasonId == seasonId)
                .Sum(zone => zone.TotalGalPerYear);

            var season = await context.Season.FindAsync(seasonId);
            season!.TotalGalPerWeek = totalWeek;
            season!.TotalGalPerMonth = totalMonth;
            season!.TotalGalPerYear = totalYear;
            await context.SaveChangesAsync();
        }

        public async Task RecalculateZoneGallons(int zoneId, IMSContext context)
        {
            var totalWeek = (from z in context.Zones
                             join p in context.Plants
                             on z.Id equals p.ZoneId
                             where z.Id == zoneId
                             select new
                             {
                                 ZoneTotal = p.GalsPerWkCalc * p.Quantity
                             }).Sum(x => x.ZoneTotal);
            var totalMonth = (from z in context.Zones
                              join p in context.Plants
                              on z.Id equals p.ZoneId
                              where z.Id == zoneId
                              select new
                              {
                                  ZoneTotal = (p.GalsPerWkCalc * p.Quantity) * 4
                              }).Sum(x => x.ZoneTotal);
            var totalYear = (from z in context.Zones
                             join p in context.Plants
                             on z.Id equals p.ZoneId
                             where z.Id == zoneId
                             select new
                             {
                                 ZoneTotal = (p.GalsPerWkCalc * p.Quantity) * 52
                             }).Sum(x => x.ZoneTotal);

            var zone = await context.Zones.FindAsync(zoneId);
            zone!.TotalGalPerWeek = totalWeek;
            zone!.TotalGalPerMonth = totalMonth;
            zone!.TotalGalPerYear = totalYear;
            await context.SaveChangesAsync();
        }

        public async Task RecalculateTotalPlants(int zoneId, IMSContext context)
        {
            var totalPlants = (from z in context.Zones
                               join p in context.Plants
                               on z.Id equals p.ZoneId
                               where z.Id == zoneId
                               select new
                               {
                                   plantTotal = p.Quantity
                               }).Sum(x => x.plantTotal);

            var zone = await context.Zones.FindAsync(zoneId);
            zone!.TotalPlants = totalPlants;
            await context.SaveChangesAsync();
        }
    }
}
