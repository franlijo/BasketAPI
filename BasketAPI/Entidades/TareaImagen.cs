namespace BasketAPI.Entidades
{
    public class TareaImagen: IId
    {
        public int Id { get; set; } 
        public string Nombre { get; set; } = string.Empty;
        public string Foto { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public byte Orden { get; set; }
        public int TareaId {  get; set; }
        //public Tarea Tarea { get; set; } = null!; // relacion 1:N con Tarea
    }
}
