using Microsoft.EntityFrameworkCore;

namespace BasketAPI.DTOs
{
    public class TareaImagenCreacionDTO
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int TareaId { get; set; }
        public byte Orden { get; set; }
        [Unicode(false)]
        public IFormFile? Foto { get; set; }

    }
}
