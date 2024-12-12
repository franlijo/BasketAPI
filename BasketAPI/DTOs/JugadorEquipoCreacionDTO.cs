namespace BasketAPI.DTOs
{
    public class JugadorEquipoCreacionDTO
    {
        public int Id { get; set; }
        public byte Dorsal { get; set; }
        public DateOnly FechaInicio { get; set; } 
        public DateOnly FechaFin { get; set; }
    }
}
