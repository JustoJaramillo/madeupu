using madeupu.API.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace madeupu.API.Data.Entities
{
    public class ProjectPhoto
    {
        public int Id { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Project Project { get; set; }

        [Display(Name = "Logo")]
        public Guid ImageId { get; set; }

        [Display(Name = "Logo")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? Constants.NoImage
            : $"{Constants.ProjectImage}{ImageId}";
    }
}
