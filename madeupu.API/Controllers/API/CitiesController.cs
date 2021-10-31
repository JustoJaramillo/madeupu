using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using madeupu.API.Data;
using madeupu.API.Data.Entities;
using madeupu.API.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace madeupu.API.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly DataContext _context;

        public CitiesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            return await _context.Cities.Include(x => x.Region).ThenInclude(x => x.Country).OrderBy(x => x.Name).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(int id)
        {
            var city = await _context.Cities.Include(x => x.Region).ThenInclude(x => x.Country).FirstOrDefaultAsync(x=> x.Id == id);

            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(int id, CityRequest cityRequest)
        {
            if (id != cityRequest.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Region region = await _context.Regions.Include(x => x.Country).FirstOrDefaultAsync(x=> x.Id == cityRequest.RegionId);
            if (region == null)
            {
                return BadRequest("La región no existe.");
            }

            City city = await _context.Cities.FindAsync(cityRequest.Id);
            if (city == null)
            {
                return BadRequest("La ciudad no existe.");
            }


            city.Region = region;
            city.Name = cityRequest.Name;

            _context.Entry(city).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe esta ciudad.");
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
        public async Task<ActionResult<City>> PostCity(CityRequest cityRequest)
        {
           

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Region region = await _context.Regions.Include(x => x.Country).FirstOrDefaultAsync(x => x.Id == cityRequest.RegionId);
            if (region == null)
            {
                return BadRequest("La región no existe.");
            }

            City city = new()
            {
                Name = cityRequest.Name,
                Region = region
            };

            _context.Cities.Add(city);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(city);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe esta ciudad.");
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
        public async Task<IActionResult> DeleteCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }
    }
}
