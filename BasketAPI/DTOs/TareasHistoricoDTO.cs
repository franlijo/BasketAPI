namespace BasketAPI.DTOs
{
    public class TareasHistoricoDTO
    {
        public List<TareaDTO> TareasVigentes { get; set; } = new List<TareaDTO>();  
        public List<TareaDTO> TareasNoVigentes { get; set; }=new List<TareaDTO>();  
    }
}
