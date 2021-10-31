using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Models.Request
{
    public class RatingRequest
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, 5, ErrorMessage = "Valor de módelo no válido.")]
        public int Rate { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string UserName { get; set; }
    }
}
