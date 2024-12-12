namespace BasketAPI.Entidades
{
    public class PartidoEntrenador
    {
        public int EntrenadorId { get; set; }
        public int PartidoId { get; set; }
        public string? Rol { get; set; }
        public byte Orden { get; set; }
        public Partido Partido { get; set; } = null!;
        public Entrenador Entrenador { get; set; } = null!;

    }
}
