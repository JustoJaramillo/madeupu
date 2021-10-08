using madeupu.API.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Models
{
    public class RegionViewModel : Region
    {

        [Display(Name = "País")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un país.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Country { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}
