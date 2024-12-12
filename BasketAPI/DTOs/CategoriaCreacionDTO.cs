using BasketAPI.Entidades;

namespace BasketAPI.DTOs
{
    public class CategoriaCreacionDTO
    {
        public required string Nombre { get; set; }
        public required string Genero { get; set; }
        public byte EdadMaxima { get; set; }
    }
}
