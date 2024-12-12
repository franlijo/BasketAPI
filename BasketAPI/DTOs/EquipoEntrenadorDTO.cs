using BasketAPI.Entidades;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BasketAPI.DTOs
{
    public class EquipoEntrenadorDTO
    {
        public int Id { get; set; }
        public string NombreCorto { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Rol { get; set; } = string.Empty;
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public string? Foto { get; set; }

        public byte Orden { get; set; }
    }
}

