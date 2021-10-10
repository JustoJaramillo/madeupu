using madeupu.API.Data;
using madeupu.API.Data.Entities;
using madeupu.API.Helpers;
using madeupu.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CitiesController : Controller
    {
        private readonly DataContext _context;
        private readonly IComboHelper _comboHelper;
        private readonly IConverterHelper _converterHelper;

        public CitiesController(DataContext context, IComboHelper comboHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _comboHelper = comboHelper;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Cities
                .Include(x => x.Region)
                .ThenInclude(x => x.Country)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            CityViewModel model = new CityViewModel
            {
                Regions = _comboHelper.getComboRegions()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CityViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    City city = await _converterHelper.ToCityAsync(model, true);
                    _context.Cities.Add(city);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe esta ciudad.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exeption)
                {
                    ModelState.AddModelError(string.Empty, exeption.InnerException.Message);
                }
            }

            model.Regions = _comboHelper.getComboCities();
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            City city = await _context.Cities
                .Include(x => x.Region)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (city == null)
            {
                return NotFound();
            }

            CityViewModel model = _converterHelper.ToCityViewModel(city);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CityViewModel cityViewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    City city = await _converterHelper.ToCityAsync(cityViewModel, false);
                    _context.Cities.Update(city);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { id = cityViewModel.RegionId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe esta ciudad.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exeption)
                {
                    ModelState.AddModelError(string.Empty, exeption.InnerException.Message);
                }
            }

            cityViewModel.Regions = _comboHelper.getComboRegions();
            return View(cityViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            City city = await _context.Cities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
