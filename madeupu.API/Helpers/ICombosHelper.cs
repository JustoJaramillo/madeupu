using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboCountries();
        IEnumerable<SelectListItem> GetComboRegions();
        IEnumerable<SelectListItem> GetComboCities();
        IEnumerable<SelectListItem> GetComboDocumentTypes();
        IEnumerable<SelectListItem> GetComboProyectCategories();
        IEnumerable<SelectListItem> GetComboParticipationTypes();
    }
}
