using BasketAPI.Entidades;

namespace BasketAPI.DTOs
{
    public class CategoriaDTO : IId
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Genero { get; set; }
        public byte EdadMaxima { get; set; }
    }
}
