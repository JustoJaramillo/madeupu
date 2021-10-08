using madeupu.API.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Helpers
{
    public class ComboHelper : IComboHelper
    {
        private readonly DataContext _context;

        public ComboHelper(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> getComboCities()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SelectListItem> getComboCountries()
        {
            List<SelectListItem> list = _context.Countries.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = $"{x.Id}"
            })
                .OrderBy(x => x.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un país...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> getComboDocumentTypes()
        {
            List<SelectListItem> list = _context.DocumentTypes.Select(x => new SelectListItem
            {
                Text = x.Description,
                Value = $"{x.Id}"
            })
                .OrderBy(x => x.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un tipo de documento...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> getComboParticipationTypes()
        {
            List<SelectListItem> list = _context.ParticipationTypes.Select(x => new SelectListItem
            {
                Text = x.Description,
                Value = $"{x.Id}"
            })
                .OrderBy(x => x.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un tipo de participación...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> getComboProyectCategories()
        {
            List<SelectListItem> list = _context.ProjectCategories.Select(x => new SelectListItem
            {
                Text = x.Description,
                Value = $"{x.Id}"
            })
                .OrderBy(x => x.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una categoría para el proyecto...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> getComboRegions()
        {
            List<SelectListItem> list = _context.Regions.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = $"{x.Id}"
            })
                .OrderBy(x => x.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una región...]",
                Value = "0"
            });

            return list;
        }
    }
}
