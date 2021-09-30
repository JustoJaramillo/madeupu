using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Data.Entities
{
    public class Request
    {
        public int Id { get; set; }
        public int ParticipationId { get; set; }

        [Display(Name = "Mensaje.")]
        [MaxLength(200, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Message { get; set; }
    }
}
