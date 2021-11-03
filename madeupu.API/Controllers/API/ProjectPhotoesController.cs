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
using madeupu.API.Helpers;

namespace madeupu.API.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectPhotoesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;

        public ProjectPhotoesController(DataContext context, IBlobHelper blobHelper)
        {
            _context = context;
            _blobHelper = blobHelper;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectPhoto>> PostProjectPhoto(ProjectPhotoRequest request)
        {

            Project project = await _context.Projects.FindAsync(request.ProjectId);

            if (project == null)
            {
                return BadRequest("El proyecto no existe");
            }

            Guid imageId = await _blobHelper.UploadBlobAsync(request.Image, "projects");
            ProjectPhoto projectPhoto = new()
            {
                ImageId = imageId,
                Project = project
            };

            _context.ProjectPhotos.Add(projectPhoto);
            await _context.SaveChangesAsync();

            return Ok("Foto agregada con exito");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectPhoto(int id)
        {
            var projectPhoto = await _context.ProjectPhotos.FindAsync(id);
            if (projectPhoto == null)
            {
                return NotFound();
            }

            await _blobHelper.DeleteBlobAsync(projectPhoto.ImageId, "projects");
            _context.ProjectPhotos.Remove(projectPhoto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectPhotoExists(int id)
        {
            return _context.ProjectPhotos.Any(e => e.Id == id);
        }
    }
}
