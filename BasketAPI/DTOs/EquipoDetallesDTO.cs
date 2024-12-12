namespace BasketAPI.DTOs
{
    public class EquipoDetallesDTO: EquipoDTO
    {
        public List<EquipoJugadorDTO> Jugadores { get; set; } = new List<EquipoJugadorDTO>();
        public List<EquipoEntrenadorDTO> Entrenadores { get; set; } = new List<EquipoEntrenadorDTO>();
    }
}
