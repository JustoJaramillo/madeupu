﻿using madeupu.API.Data;
using madeupu.API.Data.Entities;
using madeupu.API.Helpers;
using madeupu.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace madeupu.API.Controllers
{
    public class ParticipationsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _comboHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;

        public ParticipationsController(DataContext context, ICombosHelper comboHelper, IConverterHelper converterHelper, IUserHelper userHelper)
        {
            _context = context;
            _comboHelper = comboHelper;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Participations
                .Include(x => x.ParticipationType)
                .Include(x => x.Project)
                .Include(x => x.User)
                .ToListAsync());
        }

        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Project project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            ParticipationViewModel model = new ParticipationViewModel
            {
                ParticipationTypes = _comboHelper.GetComboParticipationTypes(),
                ProjectId = project.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, ParticipationViewModel model)
        {
            if (ModelState.IsValid)
            {
                Project project = await _context.Projects
                    .Include(x => x.Participations)
                    .ThenInclude(x => x.User)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (project == null)
                {
                    return NotFound();
                }

                User user = await _userHelper.GetUserAsync(model.Email);

                if (user == null)
                {
                    return NotFound();
                }

                Participation participation = await _converterHelper.ToParticipationAsync(model, true);

                participation.User = user;
                participation.Project = project;

                if (project.Participations == null)
                {
                    project.Participations = new List<Participation>();
                }

                //if (user.Participations == null)
                //{
                //    user.Participations = new List<Participation>();
                //}


                _context.Participations.Add(participation);
                project.Participations.Add(participation);
                //user.Participations.Add(participation);
                _context.Projects.Update(project);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                //return RedirectToAction("SingleProject", "Projects");
                return RedirectToAction(nameof(Index));


            }
            model.ParticipationTypes = _comboHelper.GetComboParticipationTypes();
            return View(model);
        }

        // GET: ParticipationTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participations = await _context.Participations.FindAsync(id);
            if (participations == null)
            {
                return NotFound();
            }
            return View(participations);
        }

        // POST: ParticipationTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Participation participation)
        {
            if (id != participation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este tipo de participación.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(participation);
        }

        // GET: ParticipationTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participations = await _context.Participations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (participations == null)
            {
                return NotFound();
            }

            _context.Participations.Remove(participations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
