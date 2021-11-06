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
using madeupu.API.Models.Request;

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
        public async Task<IActionResult> PutParticipation(int id, ParticipationRequest request)
        {

            if (id != request.Id)
            {
                return BadRequest();
            }

            Participation participation = await _context.Participations.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == request.Id);

            User user = await _context.Users.Include(x => x.DocumentType).FirstOrDefaultAsync(x => x.UserName == request.UserName);
            if (user == null)
            {
                return BadRequest("El usuario no existe.");
            }

            Project project = await _context.Projects.FindAsync(request.ProjectId);

            if (project == null)
            {
                return BadRequest("El proyecto no existe.");
            }

            ParticipationType participationType = await _context.ParticipationTypes.FindAsync(request.ParticipationTypeId);

            if (participationType == null)
            {
                return BadRequest("No existe el tipo de participación.");
            }

            if (participationType.Description == "Creador")
            {
                return BadRequest("Ya existe un creador para este proyecto");
            }

            participation.User = user;
            participation.ParticipationType = participationType;
            participation.Project = project;
            participation.Message = request.Message;
            participation.ActiveParticipation = request.ActiveParticipation;

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

            return Ok("Participación actualizada con exito");
        }

        [HttpPost]
        public async Task<ActionResult<Participation>> PostParticipation(ParticipationRequest request)
        {
            User user = await _context.Users.Include(x => x.DocumentType).FirstOrDefaultAsync(x => x.UserName == request.UserName);
            if (user == null)
            {
                return BadRequest("El usuario no existe.");
            }

            Project project = await _context.Projects.FindAsync(request.ProjectId);

            if (project == null)
            {
                return BadRequest("El proyecto no existe.");
            }

            ParticipationType participationType = await _context.ParticipationTypes.FindAsync(request.ParticipationTypeId);

            if (participationType == null)
            {
                return BadRequest("No existe el tipo de participación.");
            }

            if (participationType.Description == "Creador")
            {
                return BadRequest("Ya esxiste un creador para este proyecto");
            }

            Participation participation = new Participation
            {
                User = user,
                ParticipationType = participationType,
                Project = project,
                Message = request.Message,
                ActiveParticipation = request.ActiveParticipation
            };

            _context.Participations.Add(participation);
            await _context.SaveChangesAsync();

            return Ok(participation);
        }

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

            return Ok("Participación eliminada con exito");
        }

        private bool ParticipationExists(int id)
        {
            return _context.Participations.Any(e => e.Id == id);
        }


        [HttpPost]
        [Route("SendParticipationRequest")]
        public async Task<ActionResult<Participation>> PostSendParticipationRequest(ParticipationRequest request)
        {
            User user = await _context.Users.Include(x => x.DocumentType).FirstOrDefaultAsync(x => x.UserName == request.UserName);
            if (user == null)
            {
                return BadRequest("El usuario no existe.");
            }

            Project project = await _context.Projects.Include(x => x.Participations).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id == request.ProjectId);

            if (project == null)
            {
                return BadRequest("El proyecto no existe.");
            }
            if (project.Participations.Count() > 0)
            {
                foreach (var item in project.Participations)
                {
                    if (item.User.UserName == user.UserName)
                    {
                        if (item.ActiveParticipation == true)
                        {
                            return BadRequest("Ya estas participando en este proyecto");
                        }
                        else
                        {
                            return BadRequest("Ya enviaste una solicitud para participar, porfavor ten paciencia y espera una respuesta");
                        }
                    }
                }
            }

            ParticipationType participationType = await _context.ParticipationTypes.FindAsync(request.ParticipationTypeId);

            if (participationType == null)
            {
                return BadRequest("No existe el tipo de participación.");
            }

            if (participationType.Description == "Creador")
            {
                return BadRequest("Ya esxiste un creador para este proyecto");
            }

            Participation participation = new Participation
            {
                User = user,
                ParticipationType = participationType,
                Project = project,
                Message = request.Message,
                ActiveParticipation = false
            };

            _context.Participations.Add(participation);
            await _context.SaveChangesAsync();

            return Ok(participation);
        }

        [HttpPut]
        [Route("AcceptParticipationRequest/{id}")]
        public async Task<IActionResult> AcceptParticipationRequest(int id)
        {

            Participation participation = await _context.Participations.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);

            if (participation == null)
            {
                return BadRequest("No existe la participación buscada");
            }

            participation.ActiveParticipation = true;

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

            return Ok("Se ha aceptado que el usuario participe en el proyecto");
        }
    }
}
