namespace BasketAPI.Entidades
{
    public class ElementoTecnico : IId
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public List<TareaElementoTecnico> TareasElementosTecnicos { get; set; } = new(); // Relación N:M con Tarea

    }
}