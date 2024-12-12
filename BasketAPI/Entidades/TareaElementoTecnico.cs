namespace BasketAPI.Entidades
{
    public class TareaElementoTecnico
    {
        public int TareaId { get; set; }
        public Tarea Tarea { get; set; } = null!; 
        public int ElementoTecnicoId { get; set; }
        public ElementoTecnico ElementoTecnico { get; set; } = null!;
        

}
}
