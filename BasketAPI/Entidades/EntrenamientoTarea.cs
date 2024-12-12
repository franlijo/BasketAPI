using System.ComponentModel.DataAnnotations;

namespace BasketAPI.Entidades
{
    public class EntrenamientoTarea
    {
        public int TareaId { get; set; }
        public int EntrenamientoId { get; set; }
        public byte Tiempo { get; set; }
        public byte Orden { get; set; }
        [StringLength(500)]
        public string? Notas  { get; set; }
    }
}
