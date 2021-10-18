using madeupu.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Ciudad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una ciudad.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int CityId { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }

        [Display(Name = "Categoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una categoria.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ProjectCategoryId { get; set; }

        public IEnumerable<SelectListItem> ProjectCategories { get; set; }

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
        [MaxLength(800, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        [Display(Name = "Logo")]
        public Guid ImageId { get; set; }

        [Display(Name = "Logo")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Logo")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? Constants.NoImage
            : $"{Constants.ProjectImage}{ImageId}";
    }
}
