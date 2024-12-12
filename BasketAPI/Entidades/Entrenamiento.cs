namespace BasketAPI.Entidades
{
    public class Entrenamiento : IId
    {
        public int Id { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }
        public string? Direccion { get; set; }
        public int EquipoId { get; set; }

    }
}
