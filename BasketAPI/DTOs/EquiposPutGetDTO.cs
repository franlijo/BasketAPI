namespace BasketAPI.DTOs
{
    public class EquiposPutGetDTO
    {
        public EquipoDTO Equipo { get; set; } = null!;
        public List<EquipoJugadorDTO> Jugadores { get; set; } = new List<EquipoJugadorDTO>();
        public List<EquipoEntrenadorDTO> Entrenadores { get; set; } = new List<EquipoEntrenadorDTO>();
    }
}
