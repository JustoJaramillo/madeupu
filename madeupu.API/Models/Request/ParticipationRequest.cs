using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Models.Request
{
    public class ParticipationRequest
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ParticipationTypeId { get; set; }

        [MaxLength(500, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Message { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ProjectId { get; set; }
    }
}
