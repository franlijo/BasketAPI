using System.ComponentModel.DataAnnotations;

namespace BasketAPI.DTOs
{
    public class PartidoCreacionDTO
    {
        public DateTime HoraInicio { get; set; }
        public required string Direccion { get; set; }
        public string? Rival { get; set; }
        public string? Cronica { get; set; }
        public string? Resultado { set; get; }
        public int EquipoId { get; set; }
    }
}
