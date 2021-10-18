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

        Task<City> ToCityAsync(CityViewModel model, bool isNew);

        CityViewModel ToCityViewModel(City city);

        Task<User> ToUserAsync(UserViewModel model, Guid imageId, bool isNew);
        UserViewModel ToUserViewModel(User user);

        Task<Project> ToProjectAsync(ProjectViewModel model, Guid imageId, bool isNew);

        ProjectViewModel ToProjectViewModel(Project project);
    }
}
