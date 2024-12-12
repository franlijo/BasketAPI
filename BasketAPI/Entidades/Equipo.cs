using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BasketAPI.Entidades
{
    public class Equipo : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public required string Nombre { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;
        [Required]
        [StringLength(25)]
        public required string Liga { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin {  get; set; }
        public byte MinimoJugadores { get; set; }
        public byte MaximoJugadores  { get; set; }
        [Unicode(false)]
        public string? Foto { get; set; }
        public List<EquipoJugador> EquiposJugadores { get; set; } = new List<EquipoJugador>();
        public List<EquipoEntrenador> EquiposEntrenadores { get; set; } = new List<EquipoEntrenador>();
        

    }
}
