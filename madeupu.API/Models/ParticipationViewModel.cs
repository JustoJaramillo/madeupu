using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Models
{
    public class ParticipationViewModel
    {
        public int ProjectId { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Debes introducir un email válido.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Email { get; set; }

        [Display(Name = "Mensaje.")]
        [MaxLength(500, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Message { get; set; }

        [Display(Name = "Tipo de participación")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un tipo de participación.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ParticipationTypeId { get; set; }

        public IEnumerable<SelectListItem> ParticipationTypes { get; set; }
    }
}
