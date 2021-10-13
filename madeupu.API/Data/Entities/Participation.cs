﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Data.Entities
{
    public class Participation
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de participación.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public ParticipationType ParticipationType { get; set; }

        [Display(Name = "Mensaje.")]
        [MaxLength(500, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Message { get; set; }
    }
}