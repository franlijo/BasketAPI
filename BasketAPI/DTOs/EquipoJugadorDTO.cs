namespace BasketAPI.DTOs
{
    public class EquipoJugadorDTO
    {
        public int Id { get; set; }
        public string NombreCorto { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public string? Foto { get; set; }
        public byte Dorsal { get; set; }

    }
}
