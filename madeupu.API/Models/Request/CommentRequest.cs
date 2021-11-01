using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Models.Request
{
    public class CommentRequest
    {

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Message { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string UserName { get; set; }
    }
}
