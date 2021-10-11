using madeupu.API.Data;
using madeupu.API.Data.Entities;
using madeupu.API.Helpers;
using madeupu.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace madeupu.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RegionsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _comboHelper;
        private readonly IConverterHelper _converterHelper;

        public RegionsController(DataContext context, ICombosHelper comboHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _comboHelper = comboHelper;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Regions
                .Include(x => x.Country)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            RegionViewModel model = new RegionViewModel
            {
                Countries = _comboHelper.GetComboCountries()
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

            model.Countries = _comboHelper.GetComboCountries();
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Region region = await _context.Regions
                .Include(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == id);


            if (region == null)
            {
                return NotFound();
            }

            RegionViewModel model = _converterHelper.ToRegionViewModel(region);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RegionViewModel regionViewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Region region = await _converterHelper.ToRegionAsync(regionViewModel, false);
                    _context.Regions.Update(region);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { id = regionViewModel.CountryId });
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

            regionViewModel.Countries = _comboHelper.GetComboCountries();
            return View(regionViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Region region = await _context.Regions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (region == null)
            {
                return NotFound();
            }

            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
