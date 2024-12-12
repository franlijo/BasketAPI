using BasketAPI.Entidades;

namespace BasketAPI.DTOs
{
    public class EntrenamientoCreacionDTO
    {
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }
        public string? Direccion { get; set; }
        public int EquipoId { get; set; }

    }
}
