using BasketAPI.Entidades;

namespace BasketAPI.DTOs
{
    public class TareaImagenDTO : IId
    {
        public int Id { get; set; }
        public string Foto { get; set; } = string.Empty;
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int TareaId { get; set; }
        public byte Orden { get; set; }
    }
}
