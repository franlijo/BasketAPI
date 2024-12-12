namespace BasketAPI.DTOs
{
    public class EntrenadorEquipoCreacionDTO
    {
        public int Id { get; set; }
        public required string Rol { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
    }
}
