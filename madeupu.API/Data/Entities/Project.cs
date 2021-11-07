using madeupu.API.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
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
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime BeginAt { get; set; }

        [Display(Name = "Decripción")]
        [MaxLength(5000, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        //[Display(Name = "Logo")]
        //public Guid ImageId { get; set; }

        //[Display(Name = "Logo")]
        //public string ImageFullPath => ImageId == Guid.Empty
        //    ? Constants.NoImage
        //    : $"{Constants.ProjectImage}{ImageId}";

        public ICollection<ProjectPhoto> ProjectPhotos { get; set; }

        [Display(Name="Logo")]
        public string ImageFullPath => ProjectPhotos == null || ProjectPhotos.Count == 0
            ? Constants.NoImage
            : ProjectPhotos.FirstOrDefault().ImageFullPath;

        public ICollection<Comment> Comments { get; set; }


        public ICollection<Participation> Participations { get; set; }


        public ICollection<Rating> Ratings { get; set; }

        [Display(Name = "Número de calificaciones")]
        public int RatingsNumber => Ratings == null ? 0 : Ratings.Count;

        [Display(Name = "Calificación promedio")]
        //[DisplayFormat(DataFormatString = "{0:N}")]
        public int AverageRating => Ratings == null ? 0 : (Ratings.Count > 0 ? (Decimal.ToInt32(Ratings.Sum(x => x.Rate) / Ratings.Count)) : 0);

        [Display(Name = "Vídeo")]
        [MaxLength(200, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string video { get; set; }

        public string VideoCode => (video.Split('/')[video.Split('/').Count() - 1].Contains("watch") ? video.Split('/')[video.Split('/').Count() - 1].Split('=')[1] : "no lo contiene");
    }
}
