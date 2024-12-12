using System.ComponentModel.DataAnnotations;

namespace BasketAPI.Entidades
{
    public class EntrenamientoEntrenador
    {
        public int EntrenamientoId { get; set; }
        public int EntrenadorId { get; set; }
        [StringLength(15)]
        public string Rol { get; set; } = string.Empty;
        public byte Orden { get; set; }
        public Entrenamiento Entrenamiento { get; set; } = null!;
        public Entrenador Entrenador { get; set; } = null!;

    }
}
