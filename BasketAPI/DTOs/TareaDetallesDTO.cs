namespace BasketAPI.DTOs
{
    public class TareaDetallesDTO: TareaDTO
    {
        public List<CategoriaDTO> Categorias { get; set; } = new List<CategoriaDTO>();
        public List<ElementoTecnicoDTO> ElementosTecnicos { get; set; } = new List<ElementoTecnicoDTO>();
        public List<TareaImagenDTO> TareaImagenes { get; set; } = new List<TareaImagenDTO>();
    }
}
