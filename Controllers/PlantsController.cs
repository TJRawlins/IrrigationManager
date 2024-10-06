using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IrrigationManager.Data;
using IrrigationManager.Models;
using System.Security.Policy;
using IrrigationManager.Controllers;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.SqlClient;

namespace IrrigationManager.Controllers
{
    public class PlantsController : BaseApiController
    {
        private readonly IMSContext _context;

        public PlantsController(IMSContext context)
        {
            _context = context;
        }

        // GET: api/Plants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlant()
        {
          if (_context.Plants == null)
          {
              return NotFound();
          }
            return await _context.Plants.ToListAsync();
        }

        // GET: api/Plants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plant>> GetPlant(int id)
        {
          if (_context.Plants == null)
          {
              return NotFound();
          }
            var plant = await _context.Plants.FindAsync(id);

            if (plant == null)
            {
                return NotFound();
            }

            return plant;
        }


        // PUT: api/Plants/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlant(int id, Plant plant)
        {
            if (id != plant.Id)
            {
                return BadRequest();
            }
            var zone = await _context.Zones.FindAsync(plant.ZoneId);
            var seasonId = zone!.SeasonId;
            _context.Entry(plant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await RecalculateZoneGallons(plant.ZoneId);
                await RecalculateTotalPlants(plant.ZoneId);
                await RecalculateSeasonGallons(seasonId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Plants
        [HttpPost]
        public async Task<ActionResult<Plant>> PostPlant(Plant plant)
        {
          if (_context.Plants == null)
          {
              return Problem("Entity set 'IMSContext.Plant'  is null.");
          }
            var zone = await _context.Zones.FindAsync(plant.ZoneId);
            var seasonId = zone!.SeasonId;
            _context.Plants.Add(plant);
            await _context.SaveChangesAsync();
            await RecalculateZoneGallons(plant.ZoneId);
            await RecalculateTotalPlants(plant.ZoneId);
            await RecalculateSeasonGallons(seasonId);

            return CreatedAtAction("GetPlant", new { id = plant.Id }, plant);
        }

        // POST: api/Plants/copyplantstonewzone/oldZoneId/newZoneId/seasonId
        [HttpPost("copyplantstonewzone/{oldZoneId}/{newZoneId}/{seasonId}")]
        public async Task<ActionResult> CopyPlantsToNewZone(int oldZoneId, int newZoneId, int seasonId)
        {
            var plants = await _context.Plants.Where(z => z.ZoneId == oldZoneId).ToListAsync();
            if(plants.Any())
            {
                foreach (var plant in plants)
                {
                    plant.Id = 0;
                    plant.ZoneId = newZoneId;
                    plant.TimeStamp = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                }

                _context.Plants.BulkInsert(plants);
                _context.ChangeTracker.Clear();

                await RecalculateZoneGallons(newZoneId);
                await RecalculateTotalPlants(newZoneId);
                await RecalculateSeasonGallons(seasonId);

            }

            return NoContent();
        }

        // DELETE: api/Plants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            if (_context.Plants == null)
            {
                return NotFound();
            }
            var plant = await _context.Plants.FindAsync(id);
            if (plant == null)
            {
                return NotFound();
            }
            // Saving the ZoneId before SaveChagesAsync in case no plants are left after deletion
            var zoneId = plant.ZoneId;
            var zone = await _context.Zones.FindAsync(plant.ZoneId);
            var seasonId = zone!.SeasonId;
            _context.Plants.Remove(plant);
            await _context.SaveChangesAsync();
            await RecalculateZoneGallons(zoneId);
            await RecalculateTotalPlants(zoneId);
            await RecalculateSeasonGallons(seasonId);

            return NoContent();
        }

        [HttpDelete("deleteplantsfromzone/{zoneId}/{seasonId}")]
        public async Task<IActionResult> DeletePlantsFromZone(int zoneId, int seasonId)
        {
            if (_context.Plants == null)
            {
                return NotFound();
            }

            var plants = await _context.Plants.Where(z => z.ZoneId == zoneId).ToListAsync();

            if (plants.Any())
            {
                _context.BulkDelete(plants);
                _context.ChangeTracker.Clear();

                await RecalculateSeasonGallons(seasonId);
            }
            return NoContent();
        }

        private bool PlantExists(int id)
        {
            return (_context.Plants?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        /* *-*-*-*-*-*-*-*-*-* RECALCULATE TOTAL ZONE GALLONS *-*-*-*-*-*-*-*-*- */
        private async Task RecalculateZoneGallons(int zoneId) {
            var totalWeek = (from z in _context.Zones
                             join p in _context.Plants
                             on z.Id equals p.ZoneId
                             where z.Id == zoneId
                             select new {
                                 ZoneTotal = p.GalsPerWk * p.Quantity
                             }).Sum(x => x.ZoneTotal);
            var totalMonth = (from z in _context.Zones
                              join p in _context.Plants
                              on z.Id equals p.ZoneId
                              where z.Id == zoneId
                              select new {
                                  ZoneTotal = (p.GalsPerWk * p.Quantity) * 4
                              }).Sum(x => x.ZoneTotal);
            var totalYear = (from z in _context.Zones
                             join p in _context.Plants
                             on z.Id equals p.ZoneId
                             where z.Id == zoneId
                             select new {
                                 ZoneTotal = (p.GalsPerWk * p.Quantity) * 52
                             }).Sum(x => x.ZoneTotal);

            var zone = await _context.Zones.FindAsync(zoneId);
            zone!.TotalGalPerWeek = totalWeek;
            zone!.TotalGalPerMonth = totalMonth;
            zone!.TotalGalPerYear = totalYear;
            await _context.SaveChangesAsync();
        }

        /* *-*-*-*-*-*-*-*-*-* RECALCULATE TOTAL SEASON GALLONS *-*-*-*-*-*-*-*-*- */
        public async Task RecalculateSeasonGallons(int seasonId)
        {
            var totalWeek = _context.Zones
                .Where(zone => zone.SeasonId == seasonId)
                .Sum(zone => zone.TotalGalPerWeek);
            var totalMonth = _context.Zones
                .Where(zone => zone.SeasonId == seasonId)
                .Sum(zone => zone.TotalGalPerMonth);
            var totalYear = _context.Zones
                .Where(zone => zone.SeasonId == seasonId)
                .Sum(zone => zone.TotalGalPerYear);

            var season = await _context.Season.FindAsync(seasonId);
            season!.TotalGalPerWeek = totalWeek;
            season!.TotalGalPerMonth = totalMonth;
            season!.TotalGalPerYear = totalYear;
            await _context.SaveChangesAsync();
        }

        /* *-*-*-*-*-*-*-*-*-* RECALCULATE TOTAL PLANTS *-*-*-*-*-*-*-*-*- */
        private async Task RecalculateTotalPlants(int zoneId)
        {
            var totalPlants = (from z in _context.Zones
                               join p in _context.Plants
                               on z.Id equals p.ZoneId
                               where z.Id == zoneId
                               select new
                               {
                                   plantTotal = p.Quantity
                               }).Sum(x => x.plantTotal);

            var zone = await _context.Zones.FindAsync(zoneId);
            zone!.TotalPlants = totalPlants;
            await _context.SaveChangesAsync();
        }
    }
}
