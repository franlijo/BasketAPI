namespace BasketAPI.Entidades
{
    public class Categoria : IId
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Genero { get; set; }
        public byte EdadMaxima { get; set; }
        public List<Equipo> Equipos { get; set; } = new List<Equipo>();   
        public List<TareaCategoria> TareasCategorias { get; set; } = new List<TareaCategoria>();




    }
}
