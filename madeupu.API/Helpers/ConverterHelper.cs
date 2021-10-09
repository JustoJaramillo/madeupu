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
