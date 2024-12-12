namespace BasketAPI.Entidades
{
    public class EquipoJugador
    {
        public int EquipoId { get; set; }
        public int JugadorId { get; set; }
        public byte Dorsal { get; set; }
        public byte Orden { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public Equipo Equipo { get; set; } = null!;

        public Jugador Jugador { get; set; } = null!;
    }
}
