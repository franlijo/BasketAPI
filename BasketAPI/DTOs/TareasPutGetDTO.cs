namespace BasketAPI.DTOs
{
    public class TareasPutGetDTO
    {
        public TareaDTO Tarea { get; set; } = null!;
        public List<ElementoTecnicoDTO> ElementosTecnicosSeleccionados { get; set; } = new List<ElementoTecnicoDTO>();
        public List<ElementoTecnicoDTO> ElementosTecnicosNoSeleccionados { get; set; } = new List<ElementoTecnicoDTO>();
        public List<CategoriaDTO> CategoriasSeleccionadas { get; set; } = new List<CategoriaDTO>();
        public List<CategoriaDTO> CategoriasNoSeleccionadas { get; set; } = new List<CategoriaDTO>();
        public List<TareaImagenDTO> TareaImagenes { get; set; } = new List<TareaImagenDTO>();
    }
}
