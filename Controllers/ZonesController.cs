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
    public class ZonesController : BaseApiController
    {
        private readonly IMSContext _context;

        public ZonesController(IMSContext context)
        {
            _context = context;
        }

        // GET: api/Zones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Zone>>> GetZones()
        {
            if (_context.Zones == null)
            {
                return NotFound();
            }
            return await _context.Zones.ToListAsync();
        }

        // GET: api/Zones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Zone>> GetZone(int id)
        {
            if (_context.Zones == null)
            {
                return NotFound();
            }
            //var zone = await _context.Zones.FindAsync(id);

            var zone = await _context.Zones.Include(x => x.Plants).SingleOrDefaultAsync(x => x.Id == id);

            if (zone == null)
            {
                return NotFound();
            }

            return zone;
        }


        // PUT: api/Zones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{zoneId}/seasonId/{currentSeasonId}")]
        public async Task<IActionResult> PutZone(int zoneId,int currentSeasonId , Models.Zone zone)
        {
            if (zoneId != zone.Id)
            {
                return BadRequest();
            }

            _context.Entry(zone).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await RecalculateSeasonGallons(zone.SeasonId);
                if (zone.SeasonId != currentSeasonId) await RecalculateSeasonGallons(currentSeasonId);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZoneExists(zoneId))
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

        // POST: api/Zones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.Zone>> PostZone(Models.Zone zone)
        {
            if (_context.Zones == null)
            {
                return Problem("Entity set 'IMSContext.Zones'  is null.");
            }
            _context.Zones.Add(zone);
            await _context.SaveChangesAsync();
            await RecalculateSeasonGallons(zone.SeasonId);

            return CreatedAtAction("GetZone", new { id = zone.Id }, zone);
        }

        // DELETE: api/Zones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZone(int id)
        {
            if (_context.Zones == null)
            {
                return NotFound();
            }
            var zone = await _context.Zones.FindAsync(id);
            if (zone == null)
            {
                return NotFound();
            }
            var seasonId = zone.SeasonId;
            _context.Zones.Remove(zone);
            await _context.SaveChangesAsync();
            await RecalculateSeasonGallons(seasonId);

            return NoContent();
        }

        private bool ZoneExists(int id)
        {
            return (_context.Zones?.Any(e => e.Id == id)).GetValueOrDefault();
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

    }

}
