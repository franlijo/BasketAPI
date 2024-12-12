using BasketAPI.Entidades;
using BasketAPI.Utilidades;
using Microsoft.AspNetCore.Mvc;

namespace BasketAPI.DTOs
{
    public class TareaCreacionDTO
    {
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public required string Version { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
        public int? TareaPadre { get; set; }
        public required string Estado { get; set; }
        public int? EntrenadorId { get; set; }
        public string? Dominio { get; set; }
        public byte JugadoresMin { get; set; }
        public byte JugadoresMax { get; set; }
        public byte Ataque { get; set; }
        public byte Defensa { get; set; }
        public byte TiempoMin { get; set; }
        public byte TiempoMax { get; set; }
        public byte NivelFisico { get; set; }
        public byte TecnicoIndividual { get; set; }
        public byte TecnicoColectivo { get; set; }
        public byte TacticoIndividual { get; set; }
        public byte TacticoColectivo { get; set; }
        public IFormFile? Foto { get; set; }
        public string? Video {  get; set; }
        public string? Material { get; set; }
        public string? ObjetivoPrincipal { get; set; }
        public string? ObjetivoSecundario { get; set; }
        public string? Comentario { get; set; }

        //[ModelBinder(BinderType = typeof(TypeBinder))]
        //public List<TareaImagenCreacionDTO>? TareaImagenesIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder))]
        public List<int>? CategoriasIds { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder))]
        public List<int>? ElementosTecnicosIds { get; set; }





    }
}
