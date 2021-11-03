using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Models.Request
{
    public class ProjectPhotoRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public byte[] Image { get; set; }
    }
}
