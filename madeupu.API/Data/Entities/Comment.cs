using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace madeupu.API.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        [Display(Name = "Comentario")]
        [MaxLength(1000, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Message { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime Date { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime DateLocal => Date.ToLocalTime();

        [JsonIgnore]
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public User User { get; set; }

        [Display(Name = "Proyecto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Project Project { get; set; }

    }
}
