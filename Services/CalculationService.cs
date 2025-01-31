using IrrigationManager.Data;
using IrrigationManager.Interfaces;
using IrrigationManager.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

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

        public async Task CalculateGallonsPerWeek(int zoneId, IMSContext context)
        {
            var zone = await context.Zones.Include(x => x.Plants).SingleOrDefaultAsync(x => x.Id == zoneId);
            //var zone = zoneObj.Value;

            var epsilon = 2.2204460492503131e-16;

            if (zone != null)
            {
                var plantList = zone.Plants;
                if (plantList != null)
                {
                    foreach (Plant p in plantList)
                    {
                        var totalMins = zone.RuntimeHours * 60 + zone.RuntimeMinutes;
                        var totalGPH = p.EmitterGPH * p.EmittersPerPlant;
                        var gallonsPerMinute = totalGPH / 60;
                        var totalGallonsPerDay = gallonsPerMinute * totalMins;
                        var totalGallonsPerWeek = (double)(totalGallonsPerDay * zone.RuntimePerWeek);
                        p.GalsPerWkCalc = (decimal)Math.Round((totalGallonsPerWeek + epsilon) * 100) / 100;
                    }
                }
            }
     
            await context.SaveChangesAsync();
        }

        public async Task<ActionResult<Zone>> GetZone(int zoneId, IMSContext context)
        {
            var zone = await context.Zones.Include(x => x.Plants).SingleOrDefaultAsync(x => x.Id == zoneId);

            if (zone != null)
            {
                return zone;
            }
            else
            {
                return new NotFoundResult();
            }
        }
    }
}
