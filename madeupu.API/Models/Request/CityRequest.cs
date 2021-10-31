using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Models.Request
{
    public class CityRequest
    {
        public int Id { get; set; }

        [Display(Name = "Ciudad")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }


        [Display(Name = "Región")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una región.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int RegionId { get; set; }
    }
}
