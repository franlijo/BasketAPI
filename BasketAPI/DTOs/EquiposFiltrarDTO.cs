namespace BasketAPI.DTOs
{
    public class EquiposFiltrarDTO
    {
        public int Pagina { get; set; }
        public int RecordsPorPagina { get; set; }
        internal PaginacionDTO Paginacion
        {
            get
            {
                return new PaginacionDTO { Pagina = Pagina, RecordsPorPagina = RecordsPorPagina };
            }
        }
        public string?  Nombre { get; set; }
        public int CategoriaId { get; set; }
        public bool EquipoTemporada { get; set; }
        public bool EquipoHistorico { get; set;  }
    }
}
