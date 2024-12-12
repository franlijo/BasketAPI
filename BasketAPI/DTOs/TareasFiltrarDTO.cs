namespace BasketAPI.DTOs
{
    public class TareasFiltrarDTO
    {
        public int Pagina { get; set; }
        public int RecordsPorPagina { get; set; }
        internal PaginacionDTO Paginacion
        {
            get
            {
                return new PaginacionDTO { Pagina = Pagina, RecordsPorPagina = Pagina };
            }
        }
        public string? Nombre { get; set; }
        public int ElementoTecnicoId { get; set; }
        public int CategoriaId { get; set; }
    }
}
