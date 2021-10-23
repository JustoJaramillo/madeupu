using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using madeupu.API.Data;
using madeupu.API.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace madeupu.API.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentTypesController : ControllerBase
    {
        private readonly DataContext _context;

        public DocumentTypesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentType>>> GetDocumentTypes()
        {
            return await _context.DocumentTypes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentType>> GetDocumentType(int id)
        {
            var documentType = await _context.DocumentTypes.FindAsync(id);

            if (documentType == null)
            {
                return NotFound();
            }

            return documentType;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocumentType(int id, DocumentType documentType)
        {
            if (id != documentType.Id)
            {
                return BadRequest();
            }

            _context.Entry(documentType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentTypeExists(id))
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
        public async Task<ActionResult<DocumentType>> PostDocumentType(DocumentType documentType)
        {
            _context.DocumentTypes.Add(documentType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocumentType", new { id = documentType.Id }, documentType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumentType(int id)
        {
            var documentType = await _context.DocumentTypes.FindAsync(id);
            if (documentType == null)
            {
                return NotFound();
            }

            _context.DocumentTypes.Remove(documentType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocumentTypeExists(int id)
        {
            return _context.DocumentTypes.Any(e => e.Id == id);
        }
    }
}
