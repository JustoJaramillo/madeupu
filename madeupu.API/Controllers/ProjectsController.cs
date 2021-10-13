using madeupu.API.Data;
using madeupu.API.Data.Entities;
using madeupu.API.Helpers;
using madeupu.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public ProjectsController(DataContext context, ICombosHelper combosHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects
                .Include(x => x.ProjectCategory)
                .Include(x => x.City)
                .ThenInclude(x => x.Region)
                .ThenInclude(x => x.Country)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            ProjectViewModel model = new ProjectViewModel
            {
                ProjectCategories = _combosHelper.GetComboProyectCategories(),
                Countries = _combosHelper.GetComboCountries(),
                Regions = _combosHelper.GetComboRegions(),
                Cities = _combosHelper.GetComboCities()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Project project = await _converterHelper.ToProjectAsync(model, true);
                    _context.Projects.Add(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un proyecto con este nombre, intenta con uno nuevo.");
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
            
            model.ProjectCategories = _combosHelper.GetComboProyectCategories();
            model.Countries = _combosHelper.GetComboCountries();
            model.Regions = _combosHelper.GetComboRegions();
            model.Cities = _combosHelper.GetComboCities();

            return View(model);
        }

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    Region region = await _context.Regions
        //        .Include(x => x.Country)
        //        .FirstOrDefaultAsync(x => x.Id == id);


        //    if (region == null)
        //    {
        //        return NotFound();
        //    }

        //    RegionViewModel model = _converterHelper.ToRegionViewModel(region);
        //    return View(model);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, RegionViewModel regionViewModel)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            Region region = await _converterHelper.ToRegionAsync(regionViewModel, false);
        //            _context.Regions.Update(region);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index), new { id = regionViewModel.CountryId });
        //        }
        //        catch (DbUpdateException dbUpdateException)
        //        {
        //            if (dbUpdateException.InnerException.Message.Contains("duplicate"))
        //            {
        //                ModelState.AddModelError(string.Empty, "Ya existe esta region.");
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

        //    regionViewModel.Countries = _comboHelper.getComboCountries();
        //    return View(regionViewModel);
        //}

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    Region region = await _context.Regions
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (region == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Regions.Remove(region);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
