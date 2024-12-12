namespace BasketAPI.Entidades
{
    public class TareaCategoria
    {
        public int CategoriaId { get; set; }
        public  int TareaId { get; set; }
        public Categoria Categoria { get; set; } = null!;
        public Tarea Tarea { get; set; } = null!;
    }
}
