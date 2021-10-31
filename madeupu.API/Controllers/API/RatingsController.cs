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
    public class RatingsController : ControllerBase
    {
        private readonly DataContext _context;

        public RatingsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostRating(RatingRequest ratingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Project project = await _context.Projects.FindAsync(ratingRequest.ProjectId);
            if (project == null)
            {
                return BadRequest("El proyecto no existe.");
            }

            User user = await _context.Users.Include(x => x.DocumentType).FirstOrDefaultAsync(x => x.UserName == ratingRequest.UserName);
            if (user == null)
            {
                return BadRequest("El usuario no existe.");
            }

            Rating rating= new()
            {
                Rate = ratingRequest.Rate,
                Date = DateTime.UtcNow,
                Project = project,
                User = user
            };

            _context.Ratings.Add(rating);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(rating);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }


    }
}
