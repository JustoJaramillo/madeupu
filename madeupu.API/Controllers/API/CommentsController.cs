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

namespace madeupu.API.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly DataContext _context;

        public CommentsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(CommentRequest commentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Project project = await _context.Projects.FindAsync(commentRequest.ProjectId);
            if (project == null)
            {
                return BadRequest("El proyecto no existe.");
            }

            User user = await _context.Users.FirstOrDefaultAsync(x=> x.UserName == commentRequest.UserName);
            if (user == null)
            {
                return BadRequest("El usuario no existe.");
            }

            Comment comment = new()
            {
                Message = commentRequest.Message,
                Date = DateTime.UtcNow,
                Project = project,
                User = user
            };

            _context.Comments.Add(comment);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(comment);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe este comentario.");
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

       
        //}
    }
}
