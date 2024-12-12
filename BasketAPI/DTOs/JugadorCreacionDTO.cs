using BasketAPI.Entidades;
using BasketAPI.Validaciones;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BasketAPI.DTOs
{
    public class JugadorCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tenero {1} o menos ")]
        [PrimeraLetraMayuscula]
        public required string Nombre { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tenero {1} o menos ")]
        [PrimeraLetraMayuscula]
        public required string Apellidos { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(12, ErrorMessage = "El campo {0} debe tenero {1} o menos ")]
        [PrimeraLetraMayuscula]
        public required string NombreCorto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public required string Genero { get; set; }
        public byte Altura { get; set; }
        public string? Puesto { get; set; }
        public string? Caracteristicas { get; set; }
        public string? Tutor { get; set; }
        public required string Email { get; set; }
        public required string Telefono { get; set; }
        [Unicode(false)]
        public IFormFile? Foto { get; set; }
        public string? Historial { get; set; }
        public string? Notas { get; set; }
    }
}
