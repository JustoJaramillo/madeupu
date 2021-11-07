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
                .Include(x => x.ProjectPhotos)
                .Include(x => x.ProjectCategory)
                .Include(x => x.City)
                .ThenInclude(x => x.Region)
                .ThenInclude(x => x.Country)
                .Include(x => x.Comments)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.DocumentType)
                .Include(x => x.Ratings)
                .ThenInclude(x => x.User)
                .ThenInclude(x=> x.DocumentType)
                .Include(x => x.Participations)
                .ThenInclude(x => x.ParticipationType)
                .Include(x => x.Participations)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.DocumentType)
                .ToListAsync();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects
                .Include(x => x.ProjectPhotos)
                .Include(x => x.ProjectCategory)
                .Include(x => x.City)
                .ThenInclude(x => x.Region)
                .ThenInclude(x => x.Country)
                .Include(x => x.Comments)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.DocumentType)
                .Include(x => x.Ratings)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.DocumentType)
                .Include(x => x.Participations)
                .ThenInclude(x => x.ParticipationType)
                .Include(x => x.Participations)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.DocumentType)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("GetProjectsByUserName/{userName}")]
        public IEnumerable<Project> GetProjectByUserName(string userName)
        {
            List<Project> projects = new List<Project>();

            List<Participation> participations = _context.Participations
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
                .Include(x => x.Project)
                .ThenInclude(x => x.ProjectPhotos)
                .Where(x => x.User.UserName == userName).ToList();

            foreach (var item in participations)
            {
                projects.Add(item.Project);
            }

            return projects;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, ProjectRequest request)
        {

            if (id != request.Id)
            {
                return BadRequest("diferentes id");
            }

            Project project = await _context.Projects.FindAsync(request.Id);

            if (project == null)
            {
                return BadRequest("El proyecto no existe.");
            }

            City city = await _context.Cities.Include(x => x.Region).ThenInclude(x => x.Country).FirstOrDefaultAsync(x => x.Id == request.CityId);

            if (city == null)
            {
                return BadRequest("La ciudad no existe.");
            }

            ProjectCategory projectCategory = await _context.ProjectCategories.FindAsync(request.ProjectCategoryId);

            if (projectCategory == null)
            {
                return BadRequest("La categoria selecionada para el proyecto no existe.");
            }

            project.Address = request.Address;
            project.BeginAt = request.BeginAt;
            project.City = city;
            project.Description = request.Description;
            project.Name = request.Name;
            project.Website = request.Website;
            project.ProjectCategory = projectCategory;

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Proyecto editado con exito");
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(ProjectRequest request)
        {
            City city = await _context.Cities.Include(x => x.Region).ThenInclude(x => x.Country).FirstOrDefaultAsync(x => x.Id == request.CityId);

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
            List<ProjectPhoto> projectPhotos = new();
            if (request.Image != null && request.Image.Length > 0)
            {
                imageId = await _blobHelper.UploadBlobAsync(request.Image, "projects");
                projectPhotos.Add(new ProjectPhoto
                {
                    ImageId = imageId
                });
            }

            Project project = new Project
            {
                Address = request.Address,
                BeginAt = request.BeginAt,
                City = city,
                Description = request.Description,
                Name = request.Name,
                Website = request.Website,
                ProjectCategory = projectCategory,
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
                Message = "Creacion del proyecto",
                ActiveParticipation = true
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
