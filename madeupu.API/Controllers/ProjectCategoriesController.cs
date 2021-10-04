using madeupu.API.Data;
using madeupu.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Controllers
{
    public class ProjectCategoriesController : Controller
    {
        private readonly DataContext _context;
        public ProjectCategoriesController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ProjectCategories.ToListAsync());
        }

        // GET: ParticipationTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectCategory projectCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectCategory);
        }

        // GET: ParticipationTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participationType = await _context.ProjectCategories.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, ProjectCategory projectCategory)
        {
            if (id != projectCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipationTypeExists(projectCategory.Id))
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
            return View(projectCategory);
        }

        // GET: ParticipationTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participationType = await _context.ProjectCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (participationType == null)
            {
                return NotFound();
            }

            _context.ProjectCategories.Remove(participationType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParticipationTypeExists(int id)
        {
            return _context.ProjectCategories.Any(e => e.Id == id);
        }

    }
}
