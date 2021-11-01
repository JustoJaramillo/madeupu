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
using madeupu.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace madeupu.API.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;

        public ProjectsController(DataContext context, IUserHelper userHelper, IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {

            return await _context.Projects
                .Include(x => x.ProjectCategory)
                .Include(x => x.City)
                .ThenInclude(x => x.Region)
                .ThenInclude(x => x.Country)
                .Include(x => x.Comments)
                .ThenInclude(x => x.User)
                .Include(x => x.Ratings)
                .ThenInclude(x => x.User)
                .Include(x => x.Participations)
                .ThenInclude(x => x.ParticipationType)
                .Include(x => x.Participations)
                .ThenInclude(x => x.User)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects
                .Include(x => x.ProjectCategory)
                .Include(x => x.City)
                .ThenInclude(x => x.Region)
                .ThenInclude(x => x.Country)
                .Include(x => x.Comments)
                .ThenInclude(x => x.User)
                .Include(x => x.Ratings)
                .ThenInclude(x => x.User)
                .Include(x => x.Participations)
                .ThenInclude(x => x.ParticipationType)
                .Include(x => x.Participations)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(ProjectRequest request)
        {
            City city = await _context.Cities.Include(x=> x.Region).ThenInclude(x=> x.Country).FirstOrDefaultAsync(x=>x.Id ==request.CityId);

            if (city == null)
            {
                return BadRequest("La ciudad no existe.");
            }

            ProjectCategory projectCategory = await _context.ProjectCategories.FindAsync(request.ProjectCategoryId);

            if (projectCategory == null)
            {
                return BadRequest("La categoria selecionada para el proyecto no existe.");
            }

            User user = await _context.Users.Include(x => x.DocumentType).FirstOrDefaultAsync(x => x.UserName == request.UserName);
            if (user == null)
            {
                return BadRequest("El usuario no existe.");
            }

            Guid imageId = Guid.Empty;
            if (request.Image != null && request.Image.Length > 0)
            {
                imageId = await _blobHelper.UploadBlobAsync(request.Image, "projects");
            }

            Project project = new Project
            {
                Address = request.Address,
                BeginAt = request.BeginAt,
                City = city,
                Description = request.Description,
                Name= request.Name,
                Website = request.Website,
                ProjectCategory = projectCategory,
                ImageId = imageId
            };

            ParticipationType participationType = await _context.ParticipationTypes
                        .FirstOrDefaultAsync(x => x.Description == "Creador");

            if (participationType == null)
            {
                return BadRequest("No fue posible asignar el tipo de participación.");
            }

            Participation participation = new Participation
            {
                User = user,
                ParticipationType = participationType,
                Project = project,
                Message = "Creacion del proyecto"
            };

            _context.Projects.Add(project);
            _context.Participations.Add(participation);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(project);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe este proyecto.");
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
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
