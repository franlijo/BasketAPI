namespace BasketAPI.Entidades
{
    public class PartidoJugador
    {
        public int JugadorId { get; set; }
        public int PartidoId { get; set; }
        public byte  Dorsal { get; set; }
        public byte Orden { get; set; }
        public Partido Partido { get; set; } = null!;
        public Jugador Jugador { get; set; } = null!;

    }
}
