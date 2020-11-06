using System.ComponentModel.DataAnnotations;

namespace WebApiCursos.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [MaxLength(500, ErrorMessage = "El número máximo de caracteres es de 500.")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        [Required(ErrorMessage = "El(la) autor(a) es requerido(a).")]
        [Display(Name = "Autor(a)")]
        public string Author { get; set; }
        [Url(ErrorMessage = "La dirección no es válida.")]
        [Display(Name = "Dirección del curso")]
        public string Uri { get; set; }
    }
}
