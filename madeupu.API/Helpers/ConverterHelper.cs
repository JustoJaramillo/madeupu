using madeupu.API.Data;
using madeupu.API.Data.Entities;
using madeupu.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly IComboHelper _comboHelper;

        public ConverterHelper(DataContext context, IComboHelper comboHelper)
        {
            _context = context;
            _comboHelper = comboHelper;
        }

        public async Task<City> ToCityAsync(CityViewModel model, bool isNew)
        {
            return new City
            {
                Region = await _context.Regions.FindAsync(model.RegionId),
                Name = model.Name,
                Id = isNew ? 0 : model.Id

            };
        }

        public CityViewModel ToCityViewModel(City city)
        {
            return new CityViewModel
            {
                RegionId = city.Region.Id,
                Regions = _comboHelper.getComboRegions(),
                Name = city.Name,
                Id = city.Id,
            };
        }

        public async Task<Project> ToProjectAsync(ProjectViewModel model, bool isNew)
        {
            return new Project
            {
                Id = isNew?0 :model.Id,
                Name = model.Name,
                Website = model.Website,
                Address = model.Address,
                BeginAt = model.BeginAt,
                Description = model.Description,
                City = await _context.Cities.FindAsync(model.CityId),
                ProjectCategory = await _context.ProjectCategories.FindAsync(model.ProjectCategoryId),
            };
        }

        public ProjectViewModel ToProjectViewModel(Project project)
        {
            return new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Website = project.Website,
                Address = project.Address,
                BeginAt = project.BeginAt,
                Description = project.Description,
                CityId = project.City.Id,
                Cities = _comboHelper.getComboCities(),
                RegionId = project.City.Region.Id,
                Regions = _comboHelper.getComboRegions(),
                CountryId = project.City.Region.Country.Id,
                Countries = _comboHelper.getComboCountries(),
                ProjectCategoryId = project.ProjectCategory.Id,
                ProjectCategories = _comboHelper.getComboProyectCategories()
                
            };
        }

        public async Task<Region> ToRegionAsync(RegionViewModel model, bool isNew)
        {
            return new Region
            {
                Country = await _context.Countries.FindAsync(model.CountryId),
                Name = model.Name,
                Id = isNew ? 0 : model.Id

            };
        }

        public RegionViewModel ToRegionViewModel(Region region)
        {
            return new RegionViewModel
            {
                CountryId = region.Country.Id,
                Countries = _comboHelper.getComboCountries(),
                Name = region.Name,
                Id = region.Id,
            };
        }
    }
}
