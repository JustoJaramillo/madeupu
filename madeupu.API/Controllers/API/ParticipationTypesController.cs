using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using madeupu.API.Data;
using madeupu.API.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace madeupu.API.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipationTypesController : ControllerBase
    {
        private readonly DataContext _context;

        public ParticipationTypesController(DataContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticipationType>>> GetParticipationTypes()
        {
            return await _context.ParticipationTypes.OrderBy(x=> x.Description).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParticipationType>> GetParticipationType(int id)
        {
            var participationType = await _context.ParticipationTypes.FindAsync(id);

            if (participationType == null)
            {
                return NotFound();
            }

            return participationType;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipationType(int id, ParticipationType participationType)
        {
            if (id != participationType.Id)
            {
                return BadRequest();
            }

            _context.Entry(participationType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe tipo de participación.");
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
        public async Task<ActionResult<ParticipationType>> PostParticipationType(ParticipationType participationType)
        {
            _context.ParticipationTypes.Add(participationType);

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetParticipationType", new { id = participationType.Id }, participationType);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe tipo de participación.");
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
        public async Task<IActionResult> DeleteParticipationType(int id)
        {
            var participationType = await _context.ParticipationTypes.FindAsync(id);
            if (participationType == null)
            {
                return NotFound();
            }

            _context.ParticipationTypes.Remove(participationType);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
