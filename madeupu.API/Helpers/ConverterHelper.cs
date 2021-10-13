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
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(DataContext context, ICombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
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
                Regions = _combosHelper.GetComboRegions(),
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
                Cities = _combosHelper.GetComboCities(),
                RegionId = project.City.Region.Id,
                Regions = _combosHelper.GetComboRegions(),
                CountryId = project.City.Region.Country.Id,
                Countries = _combosHelper.GetComboCountries(),
                ProjectCategoryId = project.ProjectCategory.Id,
                ProjectCategories = _combosHelper.GetComboProyectCategories()
                
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
                Countries = _combosHelper.GetComboCountries(),
                Name = region.Name,
                Id = region.Id,
            };
        }

        public async Task<User> ToUserAsync(UserViewModel model, Guid imageId, bool isNew)
        {
            return new User
            {
                Address = model.Address,
                Document = model.Document,
                DocumentType = await _context.DocumentTypes.FindAsync(model.DocumentTypeId),
                Email = model.Email,
                FirstName = model.FirstName,
                Id = isNew ? Guid.NewGuid().ToString() : model.Id,
                ImageId = imageId,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email,
                UserType = model.UserType,
            };
        }

        public UserViewModel ToUserViewModel(User user)
        {
            return new UserViewModel
            {
                Address = user.Address,
                Document = user.Document,
                DocumentTypeId = user.DocumentType.Id,
                DocumentTypes = _combosHelper.GetComboDocumentTypes(),
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                ImageId = user.ImageId,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType,
            };
        }
    }
}
