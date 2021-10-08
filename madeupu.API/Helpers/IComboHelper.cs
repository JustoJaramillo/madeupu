using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Helpers
{
    public interface IComboHelper
    {
        IEnumerable<SelectListItem> getComboCountries();
        IEnumerable<SelectListItem> getComboRegions();
        IEnumerable<SelectListItem> getComboCities();
        IEnumerable<SelectListItem> getComboDocumentTypes();
        IEnumerable<SelectListItem> getComboProyectCategories();
        IEnumerable<SelectListItem> getComboParticipationTypes();
    }
}
