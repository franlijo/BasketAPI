using BasketAPI.Utilidades;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BasketAPI.DTOs
{
    public class EquipoCreacionDTO
    {
        [Required]
        [StringLength(30)]
        public required string Nombre { get; set; }
        public required int CategoriaId { get; set; }
        public required string Liga { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public byte MinimoJugadores { get; set; }
        public byte MaximoJugadores { get; set; }
        public IFormFile? Foto { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder))]
        public List<JugadorEquipoCreacionDTO>? Jugadores { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder))]
        public List<EntrenadorEquipoCreacionDTO>? Entrenadores { get; set; }


    }
}
