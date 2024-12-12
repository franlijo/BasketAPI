using BasketAPI.Entidades;

namespace BasketAPI.DTOs
{
    public class EntrenamientoDTO: IId
    {
        public int Id { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }
        public string? Direccion { get; set; }
        public int EquipoId { get; set; }
        public Equipo? Equipo { get; set; }

    }
}
