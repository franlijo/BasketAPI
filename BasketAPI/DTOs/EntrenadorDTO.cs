using BasketAPI.Entidades;
using BasketAPI.Validaciones;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BasketAPI.DTOs
{
    public class EntrenadorDTO: IId
    {

        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellidos { get; set; }
        public required string NombreCorto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public required string Titulacion { get; set; }
        public required string Email { get; set; }
        public required string Telefono { get; set; }
        public string? Foto { get; set; }
        public string? Historial { get; set; }
        public string? Notas { get; set; }

    }
}
