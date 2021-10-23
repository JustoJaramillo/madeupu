using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Models
{
    public class RatingViewModel
    {
        public int ProjectId { get; set; }

        [Display(Name = "Calificación")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, 5, ErrorMessage = "Valor de módelo no válido.")]
        public int Rate { get; set; }
    }
}
