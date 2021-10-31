using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using madeupu.API.Data;
using madeupu.API.Data.Entities;
using madeupu.API.Helpers;
using madeupu.API.Models;
using madeupu.API.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace madeupu.API.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly DataContext _context;

        public RegionsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Region>>> GetRegions()
        {
            return await _context.Regions.Include(x => x.Country).OrderBy(x => x.Name).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Region>> GetRegion(int id)
        {
            var region = await _context.Regions.Include(x => x.Country).FirstOrDefaultAsync(x => x.Id == id);

            if (region == null)
            {
                return NotFound();
            }

            return region;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegion(int id, RegionRequest regionRequest)
        {
            if (id != regionRequest.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Country country = await _context.Countries.FindAsync(regionRequest.CountryId);
            if (country == null)
            {
                return BadRequest("El país no existe.");
            }

            Region region = await _context.Regions.FindAsync(regionRequest.Id);
            if (country == null)
            {
                return BadRequest("El país no existe.");
            }


            region.Country = country;
            region.Name = regionRequest.Name;

            _context.Entry(region).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe esta región.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegionRequest regionRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Country country = await _context.Countries.FindAsync(regionRequest.CountryId);
            if (country == null)
            {
                return BadRequest("El país no existe.");
            }

            Region region = new()
            {
                Name = regionRequest.Name,
                Country = country
            };

            _context.Regions.Add(region);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(region);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe esta región.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegion(int id)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegionExists(int id)
        {
            return _context.Regions.Any(e => e.Id == id);
        }
    }
}
