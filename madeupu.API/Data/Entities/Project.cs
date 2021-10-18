using madeupu.API.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Data.Entities
{
    public class Project
    {
        public int Id { get; set; }

        [Display(Name = "Ciudad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public City City { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public ProjectCategory ProjectCategory { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Display(Name = "Sitio web")]
        [MaxLength(200, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Website { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; }

        [Display(Name = "Fecha inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime BeginAt { get; set; }

        [Display(Name = "Decripción")]
        [MaxLength(5000, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        [Display(Name = "Logo")]
        public Guid ImageId { get; set; }

        [Display(Name = "Logo")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? Constants.NoImage
            : $"{Constants.ProjectImage}{ImageId}";

    }
}
