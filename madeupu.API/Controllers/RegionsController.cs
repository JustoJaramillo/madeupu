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
    public class RegionsController : Controller
    {
        private readonly DataContext _context;
        private readonly IComboHelper _comboHelper;
        private readonly IConverterHelper _converterHelper;

        public RegionsController(DataContext context, IComboHelper comboHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _comboHelper = comboHelper;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Regions
                .Include(x=> x.Country)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            RegionViewModel model = new RegionViewModel
            {
                Countries = _comboHelper.getComboCountries()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Region region = await _converterHelper.ToRegionAsync(model, true);
                    _context.Regions.Add(region);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe esta region.");
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

            model.Countries = _comboHelper.getComboCountries();
            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Country country)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Add(country);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch (DbUpdateException dbUpdateException)
        //        {
        //            if (dbUpdateException.InnerException.Message.Contains("duplicate"))
        //            {
        //                ModelState.AddModelError(string.Empty, "Ya existe este país.");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
        //            }
        //        }
        //        catch (Exception exeption)
        //        {
        //            ModelState.AddModelError(string.Empty, exeption.InnerException.Message);
        //        }
        //    }
        //    return View(country);
        //}

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    Country country = await _context.Countries.FindAsync(id);
        //    if (country == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(country);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Country country)
        //{
        //    if (id != country.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(country);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch (DbUpdateException dbUpdateException)
        //        {
        //            if (dbUpdateException.InnerException.Message.Contains("duplicate"))
        //            {
        //                ModelState.AddModelError(string.Empty, "Ya existe este país.");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
        //            }
        //        }
        //        catch (Exception exeption)
        //        {
        //            ModelState.AddModelError(string.Empty, exeption.InnerException.Message);
        //        }
        //    }
        //    return View(country);
        //}

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    Country country = await _context.Countries
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (country == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Countries.Remove(country);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
