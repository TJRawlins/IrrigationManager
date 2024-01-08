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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlant(int id, Plant plant)
        {
            if (id != plant.Id)
            {
                return BadRequest();
            }

            _context.Entry(plant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await RecalculateZoneGallons(plant.ZoneId);
                await RecalculateTotalPlants(plant.ZoneId);
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Plant>> PostPlant(Plant plant)
        {
          if (_context.Plants == null)
          {
              return Problem("Entity set 'IMSContext.Plant'  is null.");
          }
            _context.Plants.Add(plant);
            await _context.SaveChangesAsync();
            await RecalculateZoneGallons(plant.ZoneId);
            await RecalculateTotalPlants(plant.ZoneId);

            return CreatedAtAction("GetPlant", new { id = plant.Id }, plant);
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
            _context.Plants.Remove(plant);
            await _context.SaveChangesAsync();
            await RecalculateZoneGallons(zoneId);
            await RecalculateTotalPlants(zoneId);

            return NoContent();
        }

        private bool PlantExists(int id)
        {
            return (_context.Plants?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        /* *-*-*-*-*-*-*-*-*-* RECALCULATE TOTAL GALLONS *-*-*-*-*-*-*-*-*- */
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
