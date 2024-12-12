namespace BasketAPI.DTOs
{
    public class TareasPostGetDTO
    {
        public List<ElementoTecnicoDTO> ElementosTecnicos { get; set; } = null!;
        public List<CategoriaDTO> Categorias { get; set; } = null !;
    }
}
