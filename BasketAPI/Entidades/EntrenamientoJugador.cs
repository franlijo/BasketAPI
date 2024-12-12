using System.ComponentModel.DataAnnotations;

namespace BasketAPI.Entidades
{
    public class EntrenamientoJugador
    {
        public int EntrenamientoId { get; set; }
        public int JugadorId { get; set; }
        [StringLength(20)]
        public string? EstadoAsistencia { get; set; }
        [StringLength(500)]
        public string? Notas  { get; set; }
        [StringLength(500)]

        public string? Incidencias { get; set; }
        public Entrenamiento Entrenamiento { get; set; } = null!;

        public Jugador Jugador { get; set; } = null!;

    }
}
