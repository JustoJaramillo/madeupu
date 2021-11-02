using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace madeupu.API.Models
{
    public class ProjectPhotoViewModel
    {
        public int ProjectId { get; set; }

        [Display(Name = "Logo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public IFormFile ImageFile { get; set; }
    }
}
