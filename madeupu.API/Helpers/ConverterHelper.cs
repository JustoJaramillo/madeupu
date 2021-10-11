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
        private readonly ICombosHelper _comboHelper;

        public ConverterHelper(DataContext context, ICombosHelper comboHelper)
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
                Regions = _comboHelper.GetComboRegions(),
                Name = city.Name,
                Id = city.Id,
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
                Countries = _comboHelper.GetComboCountries(),
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
                DocumentTypes = _comboHelper.GetComboDocumentTypes(),
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
