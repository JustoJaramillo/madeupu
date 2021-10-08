using madeupu.API.Data.Entities;
using madeupu.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Helpers
{
    public interface IConverterHelper
    {
        Task<Region> ToRegionAsync(RegionViewModel model, bool isNew);

        RegionViewModel ToRegionViewModel(Region region);
    }
}
