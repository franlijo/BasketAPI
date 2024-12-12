using BasketAPI.Entidades;

namespace BasketAPI.DTOs
{
    public class ElementoTecnicoDTO :IId
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set;  }
    }
}
