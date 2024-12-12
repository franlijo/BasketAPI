using BasketAPI.Entidades;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BasketAPI.DTOs
{
    public class EquipoDTO : IId
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
        public string CategoriaGenero { get; set; } = string.Empty;
        public required string Liga { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public byte MinimoJugadores { get; set; }
        public byte MaximoJugadores { get; set; }
        public string? Foto { get; set; }
    }
}
