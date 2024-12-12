using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using BasketAPI.Validaciones;

namespace BasketAPI.Entidades
{
    public class Entrenador: IId
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tenero {1} o menos ")]
        public required string Nombre { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tenero {1} o menos ")]
        public required string Apellidos { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(12, ErrorMessage = "El campo {0} debe tenero {1} o menos ")]
        public required string NombreCorto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public required string Titulacion { get; set; }
        public required string Email { get; set; }
        public required string Telefono { get; set; }
        [Unicode(false)]
        public string? Foto { get; set; }
        public string? Historial {  get; set; }
        public string? Notas { get; set; }
        //public List<Equipo> Equipos { get; set; } = new List<Equipo>();

        
        





    }
}
