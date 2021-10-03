using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using madeupu.API.Data;
using madeupu.API.Data.Entities;

namespace madeupu.API.Controllers
{
    public class ParticipationTypesController : Controller
    {
        private readonly DataContext _context;

        public ParticipationTypesController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ParticipationTypes.ToListAsync());
        }

        // GET: ParticipationTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ParticipationTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParticipationType participationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(participationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(participationType);
        }

        // GET: ParticipationTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participationType = await _context.ParticipationTypes.FindAsync(id);
            if (participationType == null)
            {
                return NotFound();
            }
            return View(participationType);
        }

        // POST: ParticipationTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ParticipationType participationType)
        {
            if (id != participationType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipationTypeExists(participationType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(participationType);
        }

        // GET: ParticipationTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participationType = await _context.ParticipationTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (participationType == null)
            {
                return NotFound();
            }

            _context.ParticipationTypes.Remove(participationType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParticipationTypeExists(int id)
        {
            return _context.ParticipationTypes.Any(e => e.Id == id);
        }
    }
}
