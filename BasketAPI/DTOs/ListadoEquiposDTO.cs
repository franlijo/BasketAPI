namespace BasketAPI.DTOs
{
    public class ListadoEquiposDTO
    {
        public List<EquipoDTO> EquiposTemporada { get; set; } = new List<EquipoDTO>();
        public List<EquipoDTO> EquiposHistorico { get; set; } = new List<EquipoDTO>();

    }
}
