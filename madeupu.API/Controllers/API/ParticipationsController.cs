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
    public class ParticipationsController : ControllerBase
    {
        private readonly DataContext _context;

        public ParticipationsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Participations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Participation>>> GetParticipations()
        {
            return await _context.Participations
                .Include(x => x.ParticipationType)
                .Include(x => x.User)
                .Include(x => x.Project)
                .ThenInclude(x => x.ProjectCategory)
                .Include(x => x.Project)
                .ThenInclude(x => x.City)
                .ThenInclude(x => x.Region)
                .ThenInclude(x => x.Country)
                .Include(x => x.Project)
                .ThenInclude(x => x.Comments)
                .ThenInclude(x => x.User)
                .Include(x => x.Project)
                .ThenInclude(x => x.Ratings)
                .ThenInclude(x => x.User)
                .Include(x => x.Project)
                .ThenInclude(x => x.Participations)
                .ThenInclude(x => x.ParticipationType)
                .Include(x => x.Project)
                .ThenInclude(x => x.Participations)
                .ThenInclude(x => x.User)
                .ToListAsync();
        }

        // GET: api/Participations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Participation>> GetParticipation(int id)
        {
            var participation = await _context.Participations
                .Include(x => x.ParticipationType)
                .Include(x => x.User)
                .Include(x => x.Project)
                .ThenInclude(x => x.ProjectCategory)
                .Include(x => x.Project)
                .ThenInclude(x => x.City)
                .ThenInclude(x => x.Region)
                .ThenInclude(x => x.Country)
                .Include(x => x.Project)
                .ThenInclude(x => x.Comments)
                .ThenInclude(x => x.User)
                .Include(x => x.Project)
                .ThenInclude(x => x.Ratings)
                .ThenInclude(x => x.User)
                .Include(x => x.Project)
                .ThenInclude(x => x.Participations)
                .ThenInclude(x => x.ParticipationType)
                .Include(x => x.Project)
                .ThenInclude(x => x.Participations)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (participation == null)
            {
                return NotFound();
            }

            return participation;
        }

        // PUT: api/Participations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipation(int id, Participation participation)
        {
            if (id != participation.Id)
            {
                return BadRequest();
            }

            _context.Entry(participation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipationExists(id))
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

        // POST: api/Participations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Participation>> PostParticipation(Participation participation)
        {
            _context.Participations.Add(participation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParticipation", new { id = participation.Id }, participation);
        }

        // DELETE: api/Participations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipation(int id)
        {
            var participation = await _context.Participations.FindAsync(id);
            if (participation == null)
            {
                return NotFound();
            }

            _context.Participations.Remove(participation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParticipationExists(int id)
        {
            return _context.Participations.Any(e => e.Id == id);
        }
    }
}
