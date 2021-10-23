using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace madeupu.API.Data.Entities
{
    public class Rating
    {
        public int Id { get; set; }

        [Display(Name = "Calificación")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, 5, ErrorMessage = "Valor de módelo no válido.")]
        public int Rate { get; set; }

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
