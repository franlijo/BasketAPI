using System.ComponentModel.DataAnnotations;

namespace BasketAPI.Entidades
{
    public class EquipoEntrenador
    {
        public int EquipoId { get; set; }
        public int EntrenadorId { get; set; }
        [StringLength(15)]
        public string Rol { get; set; } = string.Empty;
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public byte Orden { get; set; }
        public Equipo Equipo { get; set; } = null!;
        public Entrenador Entrenador { get; set; } = null!;
    }
}
