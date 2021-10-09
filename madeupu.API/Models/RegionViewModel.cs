﻿using madeupu.API.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Models
{
    public class RegionViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Regíon")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Display(Name = "País")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un país.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Country { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}