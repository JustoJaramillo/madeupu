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
    public class ProjectCategoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public ProjectCategoriesController(DataContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectCategory>>> GetProjectCategories()
        {
            return await _context.ProjectCategories.OrderBy(x => x.Description).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectCategory>> GetProjectCategory(int id)
        {
            var projectCategory = await _context.ProjectCategories.FindAsync(id);

            if (projectCategory == null)
            {
                return NotFound();
            }

            return projectCategory;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectCategory(int id, ProjectCategory projectCategory)
        {
            if (id != projectCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe tipo de proyecto.");
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
        public async Task<ActionResult<ProjectCategory>> PostProjectCategory(ProjectCategory projectCategory)
        {
            _context.ProjectCategories.Add(projectCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectCategory", new { id = projectCategory.Id }, projectCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectCategory(int id)
        {
            var projectCategory = await _context.ProjectCategories.FindAsync(id);
            if (projectCategory == null)
            {
                return NotFound();
            }

            _context.ProjectCategories.Remove(projectCategory);

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe tipo de proyecto.");
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
    }
}
